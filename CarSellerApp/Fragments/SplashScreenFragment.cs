using Android.Views;
using CarSellerApp.Fragments.Base;
using CarSellerCore.ViewModel.Splash;

namespace CarSellerApp.Fragments;

public class SplashScreenFragment : BaseFragment<SplashScreenViewModel>
{
    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);
        return inflater.Inflate(Resource.Layout.splash_screen, container, false);
    }
}