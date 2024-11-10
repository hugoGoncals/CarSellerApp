namespace CarSellerCore.Model;

public class FilterOption
{
    public string Title { get; set; }
    
    public FilterType Type { get; set; }
    
    public bool IsSelected { get; set; }
}

public enum FilterType
{
    Make,
    Model,
    Favorite,
    StartingBid,
    Mileage,
    AuctionDate,
}