using Android;
using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Navigation.Fragment;
using CarSellerCore.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Essentials;

namespace CarSellerApp;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        DIContainer.Configure(); // Initialize DI
        Platform.Init(this, savedInstanceState);

        SetContentView(Resource.Layout.activity_main);
        
        var navHostFragment = SupportFragmentManager.FindFragmentById(Resource.Id.my_nav_host_fragment) as NavHostFragment;
        var navController = navHostFragment?.NavController;
    }
    
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}
