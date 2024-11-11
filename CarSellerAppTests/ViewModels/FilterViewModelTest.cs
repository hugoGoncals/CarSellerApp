using CarSellerCore.Model;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Filter;
using FluentAssertions;
using Moq;

namespace CarSellerAppTests.ViewModels;

public class FilterViewModelTest
{
    private readonly Mock<ICarService> _carService = new();
    private readonly Mock<IDialogService> _dialogService = new();
    
    
    [Fact]
    public async Task SortByTest()
    {
        var vm = CreateViewModel();
        SetupCarsService(_carService);
        
        await vm.OnAppearing();
        vm.OnSortByAdded(1);

        vm.SortOptions[1].IsSelected.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("Ford", 1)]
    [InlineData("Not a veicle", 0)]
    public async Task ModelPickerTest(string maker, int count)
    {
        var vm = CreateViewModel();
        SetupCarsService(_carService);
        
        await vm.OnAppearing();
        vm.OnMakerSelected(maker);

        vm.ModelOptions.Count.Should().BeGreaterThanOrEqualTo(count);
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


    private TestFilterViewModelTest CreateViewModel() => new(_carService.Object,
        _dialogService.Object);
}

public class TestFilterViewModelTest : FilterViewModel
{
    public TestFilterViewModelTest(ICarService carService, IDialogService dialogService) : base(carService, dialogService)
    {
    }

    public override void NavigateBackToDashboard(FilterModel filters)
    {
    }
}