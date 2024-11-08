using CarSellerCore.Model;
using CarSellerCore.Services.Abstraction;

namespace CarSellerCore.ViewModel.Dashboard;

public class DashboardViewModel : BaseViewModel
{
    private readonly ICarService _carService;
    private List<Car> _carsList;

    public DashboardViewModel(ICarService carService)
    {
        _carService = carService;
    }

    public List<Car> CarsList
    {
        get => _carsList;
        set => SetProperty(ref _carsList, value);
    }

    public override async Task OnAppearing()
    {
        await base.OnAppearing();
        try
        {
            CarsList = await _carService.GetCarsAsync();
        }
        catch (Exception e)
        {
            
        } 
    }

    public string Title => "This is dashboard title";
}