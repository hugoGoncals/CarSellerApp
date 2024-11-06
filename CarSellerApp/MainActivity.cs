using Android;
using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Navigation.Fragment;
using CarSellerCore.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace CarSellerApp;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        DIContainer.Configure(); // Initialize DI

        SetContentView(Resource.Layout.activity_main);
        
        var navHostFragment = SupportFragmentManager.FindFragmentById(Resource.Id.my_nav_host_fragment) as NavHostFragment;
        var navController = navHostFragment?.NavController;
    }
}
