namespace CarSellerCore.Model;

public class FilterModel
{
    public string SelectedMaker{ get; set; }
    
    public string SelectedModel { get; set; }
    
    public (int start, int end) BindingRange {get; set; }
    
    public FilterType? SortBy { get; set; }
    
    public bool ShowOnlyFavorites { get; set; }
}