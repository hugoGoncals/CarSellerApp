using CommunityToolkit.Mvvm.ComponentModel;

namespace CarSellerCore.ViewModel;

public class BaseViewModel : ObservableObject
{
    public BaseViewModel()
    {
    }
    
    public string Teste => "CONA";
}