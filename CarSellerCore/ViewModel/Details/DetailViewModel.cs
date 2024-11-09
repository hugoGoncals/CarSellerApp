using CarSellerCore.Model;
using CarSellerCore.Services;
using CarSellerCore.Services.Abstraction;

namespace CarSellerCore.ViewModel.Details;

public class DetailViewModel : BaseViewModel
{
    public DetailViewModel(ICarService carService) : base(carService)
    {
    }

    public int? CarId { get; set; }

    public Car Car { get; set; }

    public override async Task OnAppearing()
    {
        base.OnAppearing();
        Car ??= await CarService.GetCar(CarId ?? 0);
    }
}