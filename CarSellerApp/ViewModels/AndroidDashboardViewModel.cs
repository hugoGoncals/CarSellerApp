using CarSellerApp.Services;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Dashboard;

namespace CarSellerApp.ViewModels;

public class AndroidDashboardViewModel : DashboardViewModel
{
    private readonly INavService _navService;

    public AndroidDashboardViewModel(INavService navService, ICarService carService, IDialogService dialogService) : base(carService, dialogService)
    {
        _navService = navService;
    }

    public override void NavigateToCarDetails(int id)
    {
        var bundle = new Bundle();
        bundle.PutInt("CarId", id);
        _navService.Navigate(Resource.Id.action_dashboard_to_detail, bundle);
    }

    public override void NavigateToFilters() => _navService.Navigate(Resource.Id.action_dashboard_to_filters);
}