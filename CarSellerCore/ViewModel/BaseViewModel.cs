using CarSellerCore.Services.Abstraction;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarSellerCore.ViewModel;

public class BaseViewModel : ObservableObject
{
    protected readonly ICarService CarService;
    protected readonly IDialogService DialogService;
    
    public virtual string Title { get; }

    public virtual bool HasBackNavigation => true;

    public BaseViewModel(ICarService carService, IDialogService dialogService)
    {
        CarService = carService;
        DialogService = dialogService;
    }

    public virtual async Task OnAppearing()
    {
        
    }
    
}