using CarSellerCore.Model;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Dashboard;
using FluentAssertions;
using Moq;

namespace CarSellerAppTests.ViewModels;

public class DashboardViewModelTest
{
    private readonly Mock<ICarService> _carService = new();
    private readonly Mock<IDialogService> _dialogService = new();

    public DashboardViewModelTest()
    {
        
    }
    
    [Fact]
    public async Task CompleteListWhenInitApp()
    {
        var vm = CreateViewModel();
        SetupCarsService(_carService);
        
        await vm.OnAppearing();
        vm.FilteredList.Count.Should().Be(CarsList.Count);
    }
    
    [Fact]
    public async Task NoFavorites()
    {
        var vm = CreateViewModel();
        SetupCarsService(_carService);
        
        await vm.OnAppearing();
        await vm.OnFavorite(1, false);
        vm.FilteredList[0].Favourite.Should().Be(false);
    }

    private static void SetupCarsService(Mock<ICarService> carService)
    {
        carService
            .Setup(service => service.GetCarsAsync())
            .Returns(Task.FromResult(CarsList));
    }

    private static List<Car> CarsList => new List<Car>()
    {
        new Car()
        {
            Id = 1,
            Make = "Ford",
            Model = "Focus",
            Favourite = true,
        },
        new Car()
        {
            Id = 2,
            Make = "Honda",
            Model = "Civic",
            Favourite = false,
        },
        new Car()
        {
            Id = 3,
            Make = "BMW",
            Model = "Series 3",
            Favourite = true,
        },
        new Car()
        {
            Id = 4,
            Make = "Pegeout",
            Model = "308",
            Favourite = true,
        }
    };


    private TestDashboardViewModel CreateViewModel() => new(_carService.Object,
        _dialogService.Object);
}

public class TestDashboardViewModel : DashboardViewModel
{
    public TestDashboardViewModel(ICarService carService, IDialogService dialogService) : base(carService, dialogService)
    {
    }

    public override void NavigateToCarDetails(int id)
    {
    }

    public override void NavigateToFilters()
    {
    }
}