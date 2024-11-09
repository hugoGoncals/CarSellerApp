using CarSellerCore.Services.Abstraction;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarSellerCore.ViewModel;

public class BaseViewModel : ObservableObject
{
    protected readonly ICarService CarService;

    public BaseViewModel(ICarService carService)
    {
        CarService = carService;
        _ = InitializeViewModel();
    }

    public async Task InitializeViewModel()
    {
        
    }

    public virtual async Task OnAppearing()
    {
        
    }
    
}