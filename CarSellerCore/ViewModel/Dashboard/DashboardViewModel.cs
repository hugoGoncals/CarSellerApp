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

    public List<int> PagesLimitPerPage => new()
    {
        5,
        10,
        15
    };

    public bool Filters => CarService.FilterSelection is not null;

    public int PageLimit
    {
        get => _pageLimit;
        set => SetProperty(ref _pageLimit, value);
    }


    public DashboardViewModel(ICarService carService, IDialogService dialogService) : base(carService, dialogService)
    {
    }

    public List<Car> CarsList
    {
        get => _carsList;
        set => SetProperty(ref _carsList, value);
    }
    
    public List<Car> FilteredList { get; set; }

    public List<Car> DisplayList
    {
        get => _displayList;
        set => SetProperty(ref _displayList, value);
    }

    public override async Task OnAppearing()
    {
        await base.OnAppearing();
        await FetchCarsAsync();
    }

    private async Task FetchCarsAsync()
    {
        try
        {
            DialogService.ShowLoading();
            var cars = await CarService.GetCarsAsync();
            DialogService.HideLoading();
            
            CarsList ??= cars;
            FilteredList ??= CarsList;

            if (CarService.FilterSelection is not null)
            {
                FilterCarsForAuction();
                return;
            }
            
            DisplayList = FilteredList.Take(5).ToList();
        }
        catch (Exception e)
        {
            DialogService.HideLoading();
        } 
    }

    private void FilterCarsForAuction()
    {
        var filters = CarService.FilterSelection;

        FilteredList = new List<Car>(CarsList);

        FilteredList = FilteredList.Where(car =>
            car.StartingBid >= filters.BindingRange.start && car.StartingBid <= filters.BindingRange.end).ToList();

        if (!string.IsNullOrEmpty(filters.SelectedMaker))
        {
            FilteredList.RemoveAll(car => car.Make != filters.SelectedMaker);
        }
        
        if (!string.IsNullOrEmpty(filters.SelectedModel))
        {
            FilteredList.RemoveAll(car => car.Model != filters.SelectedModel);
        }
        
        if (filters.ShowOnlyFavorites)
        {
            FilteredList.RemoveAll(car => !car.Favourite);
        }

        if (filters.SortBy is not null)
        {
            switch (filters.SortBy)
            {
                case FilterType.Make:
                    FilteredList = FilteredList.OrderBy(car => car.Make).ToList();
                    break;
                case FilterType.Mileage:
                    FilteredList = FilteredList.OrderBy(car => car.Mileage).ToList();
                    break;
                case FilterType.StartingBid:
                    FilteredList = FilteredList.OrderBy(car => car.StartingBid).ToList();
                    break;
                case FilterType.AuctionDate:
                    FilteredList = FilteredList.OrderBy(car => car.AuctionDateTime).ToList();
                    break;
            }
        }
        
        DisplayList = FilteredList.Skip((CurrentPage - 1) * PageLimit).Take(PageLimit).ToList();
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
        if (CurrentPage * PageLimit >= FilteredList.Count)
        {
            return;
        }

        DisplayList = FilteredList.Skip(CurrentPage * PageLimit).Take(PageLimit).ToList();
        CurrentPage++;
    }

    public void PreviousPage()
    {
        if (CurrentPage <= 1)
        {
            return;
        }

        CurrentPage--;
        DisplayList = FilteredList.Skip((CurrentPage - 1) * PageLimit).Take(PageLimit).ToList();
    }

    public async Task OnFavorite(int carId, bool isFavorite)
    {
        try
        {
            var car = DisplayList.FirstOrDefault(car => car.Id == carId);

            if (car is null)
            {
                return;
            }
            
            DialogService.ShowLoading();
            DisplayList.FirstOrDefault(car => car.Id == carId).Favourite = isFavorite;
            await CarService.UpdateCar(DisplayList.FirstOrDefault(car => car.Id == carId));
            DialogService.HideLoading();
            
            OnPropertyChanged(nameof(DisplayList));
        }
        catch (Exception e)
        {
            
        }
    }

    public void OnPageLimitChanged(int pos)
    {
        var size = PagesLimitPerPage[pos];
        var oldLimit = PageLimit;
        PageLimit = size;
        DisplayList = FilteredList?.Skip((CurrentPage - 1) * oldLimit).Take(PageLimit).ToList() ?? new List<Car>();
    }
    
    public abstract void NavigateToCarDetails(int id);

    public abstract void NavigateToFilters();
}