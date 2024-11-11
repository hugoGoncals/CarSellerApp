using CarSellerApp.Services;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Splash;

namespace CarSellerApp.ViewModels;

public class AndroidSplashScreenViewModel : SplashScreenViewModel
{
    private readonly INavService _navService;

    public AndroidSplashScreenViewModel(INavService navService, ICarService carService, IDialogService dialogService) : base(carService, dialogService)
    {
        _navService = navService;
    }

    protected override void NavigateToDashboard() => _navService.Navigate(Resource.Id.actions_splash_to_dashboard);
}