using CarSellerCore.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace CarSellerApp;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        DIContainer.Configure(); // Initialize DI

        var mainViewModel = DIContainer.Services.GetService<BaseViewModel>();

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);

        var v = FindViewById<TextView>(Resource.Id.teste);
        v.Text = mainViewModel.Teste;


    }
}
