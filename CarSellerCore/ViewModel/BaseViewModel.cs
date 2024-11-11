using CarSellerCore.Services.Abstraction;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarSellerCore.ViewModel;

public class BaseViewModel : ObservableObject
{
    protected readonly ICarService CarService;
    protected readonly IDialogService DialogService;

    public BaseViewModel(ICarService carService, IDialogService dialogService)
    {
        CarService = carService;
        DialogService = dialogService;
        _ = InitializeViewModel();
    }

    public async Task InitializeViewModel()
    {
        
    }

    public virtual async Task OnAppearing()
    {
        
    }
    
}