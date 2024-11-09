using AndroidX.Navigation;

namespace CarSellerApp.Services;

public interface INavService
{
    NavController NavController { get; }

    void Init(MainActivity activity);

    void Navigate(int resId);

    void Navigate(int resId, Bundle args);

    void Navigate(int resId, Bundle args, NavOptions navOptions);
}