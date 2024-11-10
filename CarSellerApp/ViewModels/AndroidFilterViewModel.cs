using CarSellerApp.Services;
using CarSellerCore.Model;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Filter;
using Object = Java.Lang.Object;

namespace CarSellerApp.ViewModels;

public class AndroidFilterViewModel : FilterViewModel
{
    private readonly INavService _navService;

    public AndroidFilterViewModel(ICarService carService, INavService navService) : base(carService)
    {
        _navService = navService;
    }

    public override void NavigateBackToDashboard(FilterModel filters) => _navService.Pop();
}