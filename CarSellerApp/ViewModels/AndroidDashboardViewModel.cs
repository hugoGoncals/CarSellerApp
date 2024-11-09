using CarSellerApp.Services;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Dashboard;

namespace CarSellerApp.ViewModels;

public class AndroidDashboardViewModel : DashboardViewModel
{
    private readonly INavService _navService;

    public AndroidDashboardViewModel(INavService navService, ICarService carService) : base(carService)
    {
        _navService = navService;
    }

    public override void NavigateToCarDetails(int id)
    {
        var bundle = new Bundle();
        bundle.PutInt("CarId", id);
        _navService.Navigate(Resource.Id.action_firstFragment_to_secondFragment, bundle);
    }
}