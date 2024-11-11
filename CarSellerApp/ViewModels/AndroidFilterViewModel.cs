using CarSellerApp.Services;
using CarSellerCore.Model;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Filter;

namespace CarSellerApp.ViewModels;

public class AndroidFilterViewModel : FilterViewModel
{
    private readonly INavService _navService;

    public AndroidFilterViewModel(ICarService carService, INavService navService, IDialogService dialogService) : base(carService, dialogService)
    {
        _navService = navService;
    }

    public override void NavigateBackToDashboard(FilterModel filters) => _navService.Pop();
}