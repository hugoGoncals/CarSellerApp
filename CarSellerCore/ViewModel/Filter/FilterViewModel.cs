using CarSellerCore.Model;
using CarSellerCore.Services.Abstraction;

namespace CarSellerCore.ViewModel.Filter;

public abstract class FilterViewModel : BaseViewModel
{
    private (int minPrice, int maxPrice) _priceRange;
    private int _startingBidValue;
    private int _endBidValue;
    private List<string> _modelOptions = new();
    private List<string> _makerOptions = new();

    public override string Title => "Set configurations";
    
    public List<Car> Cars { get; set; }

    public string SelectedMaker { get; set; }
    
    public string SelectedModel { get; set; }

    public List<string> MakerOptions
    {
        get => _makerOptions;
        set => SetProperty(ref _makerOptions, value);
    }

    public List<string> ModelOptions
    {
        get => _modelOptions;
        set => SetProperty(ref _modelOptions, value);
    }

    public List<FilterOption> SortOptions { get; set; } = new List<FilterOption>()
    {
        new FilterOption()
        {
            Title = "Make",
            Type = FilterType.Make,
        },

        new FilterOption()
        {
            Title = "Starting Bid",
            Type = FilterType.StartingBid,
        },

        new FilterOption()
        {
            Title = "Mileage",
            Type = FilterType.Mileage,
        },

        new FilterOption()
        {
            Title = "Auction Date",
            Type = FilterType.AuctionDate,
        },
    };

    public string StartingBidLabel => $"From: {StartingBidValue}€";
    
    public string EndBidLabel => $"To: {EndBidValue}€";

    public int StartingBidValue
    {
        get => _startingBidValue;
        set
        {
            SetProperty(ref _startingBidValue, value);
            OnPropertyChanged(nameof(StartingBidLabel));
        }
    }

    public int EndBidValue
    {
        get => _endBidValue;
        set
        {
            SetProperty(ref _endBidValue, value);
            OnPropertyChanged(nameof(EndBidLabel));
        }
    }

    public (int minPrice, int maxPrice) PriceRange
    {
        get => _priceRange;
        set => SetProperty(ref _priceRange, value);
    }

    public bool IsFavorite { get; set; }

    public FilterViewModel(ICarService carService, IDialogService dialogService) : base(carService, dialogService)
    {
    }

    public override async  Task OnAppearing()
    {
        base.OnAppearing();
        try
        {
            Cars = await CarService.GetCarsAsync();
            PriceRange = (Cars.Min(car => car.StartingBid), Cars.Max(car => car.StartingBid));
            StartingBidValue = PriceRange.minPrice;
            EndBidValue = PriceRange.maxPrice;
            
            MakerOptions = Cars
                .Select(car => car.Make) 
                .Distinct()
                .ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void OnConfigurationsSetted()
    {
        var filters = new FilterModel()
        {
            SelectedMaker = SelectedMaker,
            SelectedModel = SelectedModel,
            ShowOnlyFavorites = IsFavorite,
            BindingRange = (StartingBidValue, EndBidValue),
            SortBy = SortOptions.FirstOrDefault(sort => sort.IsSelected)?.Type,
        };

        CarService.FilterSelection = filters;
        
        NavigateBackToDashboard(filters);
    }
    
    public void OnSortByAdded(int index)
    {
        var sortOption = SortOptions.ElementAtOrDefault(index);
        if (sortOption != null)
        {
            sortOption.IsSelected = !sortOption.IsSelected;
        }
    }

    public abstract void NavigateBackToDashboard(FilterModel filters);

    public void OnMakerSelected(string? maker)
    {
        SelectedMaker = maker;
        ModelOptions = Cars
            .Where(car => car.Make == maker)
            .Select(car => car.Model)
            .Distinct()
            .ToList();
    }
}