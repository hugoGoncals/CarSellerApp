namespace CarSellerCore.ViewModel.Details;

public class DetailViewModel : BaseViewModel
{
    public DetailViewModel()
    {
    }

    public int? CarId { get; set; }

    public override Task OnAppearing()
    {
        var t = CarId;
        return base.OnAppearing();
    }
}