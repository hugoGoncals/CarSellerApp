using CommunityToolkit.Mvvm.ComponentModel;

namespace CarSellerCore.ViewModel;

public class BaseViewModel : ObservableObject
{
    public BaseViewModel()
    {
        _ = InitializeViewModel();
    }

    public async Task InitializeViewModel()
    {
        
    }

    public async Task OnAppearing()
    {
        
    }
    
}