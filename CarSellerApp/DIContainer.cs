using CarSellerApp.Services;
using CarSellerApp.ViewModels;
using CarSellerCore.Services;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Dashboard;
using CarSellerCore.ViewModel.Details;
using CarSellerCore.ViewModel.Filter;
using CarSellerCore.ViewModel.Splash;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CarSellerApp;

public static class DIContainer
{
    public static void Configure()
    {
        var services = new ServiceCollection();
        RegisterServices(services);
        RegisterViewModels(services);

        Ioc.Default.ConfigureServices(services.BuildServiceProvider());
    }

    private static void RegisterServices(ServiceCollection services)
    {
        services.AddSingleton<ICarService, CarService>();
        services.AddSingleton<INavService, NavService>();
        services.AddSingleton<IDialogService, DialogService>();
    }

    private static void RegisterViewModels(ServiceCollection services)
    {
        services.AddTransient<SplashScreenViewModel, AndroidSplashScreenViewModel>();
        services.AddTransient<DashboardViewModel, AndroidDashboardViewModel>();
        services.AddTransient<FilterViewModel, AndroidFilterViewModel>();
        services.AddTransient<DetailViewModel>();
    }
}