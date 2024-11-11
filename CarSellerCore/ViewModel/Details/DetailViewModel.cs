using CarSellerCore.Model;
using CarSellerCore.Services;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.Util;

namespace CarSellerCore.ViewModel.Details;

public class DetailViewModel : BaseViewModel
{
    private int? _carId;
    private List<(string title, List<(string title, string description)> description)> _carDetailList;

    public DetailViewModel(ICarService carService) : base(carService)
    {
    }

    public int? CarId { get; set; }

    public List<(string title, List<(string title, string description)> description)> CarDetailList
    {
        get => _carDetailList;
        set => SetProperty(ref _carDetailList, value);
    }

    public Car Car { get; set; }

    public override async Task OnAppearing()
    {
        base.OnAppearing();
        Car ??= await CarService.GetCar(CarId ?? 0);
        CreateCarDetailsList();
    }

    private void CreateCarDetailsList()
    {
        CarDetailList = new List<(string section, List<(string title, string description)>)>();
        
        CarDetailList.Add(("Basic information", new List<(string title, string description)>()
        {
            ("Engine Size", Car.EngineSize),
            ("Fuel Type", Car.Fuel),
            ("Year", Car.Year.ToString()),
            ("Mileage", $"{Car.Mileage} Kilometers"),
            ("Auction Date and Time", DataUtil.FormatDate(Car.AuctionDateTime)),
            ("Starting Bid", $"{Car.StartingBid} €"),
        }));
        
        CarDetailList.Add(("Specifications", new List<(string title, string description)>()
        {
            ("Vehicle Type", Car.Details.Specification.VehicleType),
            ("Colour", Car.Details.Specification.Colour),
            ("Fuel", Car.Details.Specification.Fuel),
            ("Transmission", Car.Details.Specification.Transmission),
            ("Number Of Doors", Car.Details.Specification.NumberOfDoors.ToString()),
            ("CO2 Emissions", Car.Details.Specification.Co2Emissions),
            ("NOX Emissions", Car.Details.Specification.NoxEmissions.ToString()),
            ("Number Of Keys", Car.Details.Specification.NumberOfKeys.ToString())
        })); 
        
        CarDetailList.Add(("Ownership", new List<(string title, string description)>()
        {
            ("Logbook", Car.Details.Ownership.LogBook),
            ("Number Of Owners", Car.Details.Ownership.NumberOfOwners.ToString()),
            ("Date Of Registration", Car.Details.Ownership.DateOfRegistration),
        }));

        List<(string title, string description)> equipmentList = new List<(string title, string description)>();
        foreach (var equipment in Car.Details.Equipment)
        {
            equipmentList.Add(("", equipment));
        }
        
        CarDetailList.Add(("Equipment", equipmentList));
        OnPropertyChanged(nameof(CarDetailList));
    }
}