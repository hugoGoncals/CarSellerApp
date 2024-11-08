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

    public virtual async Task OnAppearing()
    {
        
    }
    
}