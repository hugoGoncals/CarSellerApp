namespace CarSellerApp.Services;

public interface INavService
{
    void Init(MainActivity activity);

    void Navigate(int resId);

    void Navigate(int resId, Bundle args);

    void Pop();
}