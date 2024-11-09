using AndroidX.Navigation;

namespace CarSellerApp.Services;

public class NavService : INavService
{

    private MainActivity _activity;
    
    public NavController NavController => Navigation.FindNavController(_activity, Resource.Id.my_nav_host_fragment);

    public void Init(MainActivity activity) => _activity = activity;

    public void Navigate(int resId)
    {
        Catch(() => NavController.Navigate(resId, null));
    }

    public void Navigate(int resId, Bundle args)
    {
        Catch(() => NavController.Navigate(resId, args));
    }

    public void Navigate(int resId, Bundle args, NavOptions navOptions) =>
        Catch(() => NavController.Navigate(resId, args, navOptions));
    

    private void Catch(Action func)
    {
        try
        {
            func();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.ToString());
        }
    }
}