using CarSellerCore.Model;
using CarSellerCore.Services.Abstraction;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace CarSellerCore.ViewModel.Dashboard;

public abstract class DashboardViewModel : BaseViewModel
{
    private List<Car> _carsList;
    private int _currentPage = 1;
    private int _pageLimit = 5;
    private List<Car> _displayList;

    public int CurrentPage
    {
        get => _currentPage;
        set => SetProperty(ref _currentPage, value);
    }

    private int MaxPages => CarsList.Count / 5;

    public int PageLimit
    {
        get => _pageLimit;
        set => SetProperty(ref _pageLimit, value);
    }


    public DashboardViewModel(ICarService carService) : base(carService)
    {
    }

    public List<Car> CarsList
    {
        get => _carsList;
        set => SetProperty(ref _carsList, value);
    }

    public List<Car> DisplayList
    {
        get => _displayList;
        set => SetProperty(ref _displayList, value);
    }

    public override async Task OnAppearing()
    {
        await base.OnAppearing();
        try
        {
            CarsList ??= await CarService.GetCarsAsync();

            DisplayList = CarsList.Take(5).ToList();
        }
        catch (Exception e)
        {
            
        } 
    }

    public string Title => "This is dashboard title";

    public async Task OnAssociateImage(int id)
    {
        if (CarsList.FirstOrDefault(car => car.Id == id) is null)
        {
            return;
        }
        
        try
        {
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                CompressionQuality = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                SaveMetaData = false,
                MaxWidthHeight = 1500,
            });
            
            
            if (file == null)
            {
                DisplayList.FirstOrDefault(car => car.Id == id).SelectedPhoto = null;
                return;
            }

            var stream = file.GetStream();
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                DisplayList.FirstOrDefault(car => car.Id == id).SelectedPhoto = memoryStream.ToArray();
            }

            CarService.UpdateCar(DisplayList.FirstOrDefault(car => car.Id == id));
            
            OnPropertyChanged(nameof(DisplayList));
        }
        catch (Exception e)
        {
        }
    }

    public void NextPage()
    {
        if (CurrentPage * PageLimit >= CarsList.Count)
        {
            return;
        }

        DisplayList = CarsList.Skip(CurrentPage * PageLimit).Take(PageLimit).ToList();
        CurrentPage++;
    }

    public void PreviousPage()
    {
        if (CurrentPage <= 1)
        {
            return;
        }

        CurrentPage--;
        DisplayList = CarsList.Skip((CurrentPage - 1) * PageLimit).Take(PageLimit).ToList();
    }
    
    public abstract void NavigateToCarDetails(int id);

    public void OnFavorite(int carId, bool isFavorite)
    {
        try
        {
            DisplayList.FirstOrDefault(car => car.Id == carId).Favourite = isFavorite;
            CarService.UpdateCar(DisplayList.FirstOrDefault(car => car.Id == carId));
            OnPropertyChanged(nameof(DisplayList));
        }
        catch (Exception e)
        {
            
        }
    }
}