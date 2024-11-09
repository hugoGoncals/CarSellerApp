namespace CarSellerApp.Adapter;

public interface ICarAdapterListenner
{
    void OnPhotoAdded(int id);
    
    void NavigateToDetails(int id);
}