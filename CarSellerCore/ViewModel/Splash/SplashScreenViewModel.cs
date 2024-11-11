using CarSellerCore.Services.Abstraction;

namespace CarSellerCore.ViewModel.Splash;

public abstract class SplashScreenViewModel : BaseViewModel
{
    public SplashScreenViewModel(ICarService carService, IDialogService dialogService) : base(carService, dialogService)
    {
    }
    
    public override async Task OnAppearing()
    {
        await base.OnAppearing();
        await Task.Delay(2000);
        NavigateToDashboard();
    }

    protected abstract void NavigateToDashboard();
}