using CarSellerApp.Services;
using CarSellerApp.ViewModels;
using CarSellerCore.Services;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Dashboard;
using CarSellerCore.ViewModel.Details;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CarSellerApp;

public static class DIContainer
{
    public static void Configure()
    {
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
                .AddSingleton<ICarService, CarService>()
                .AddSingleton<INavService, NavService>()
                .AddTransient<DashboardViewModel, AndroidDashboardViewModel>()
                .AddTransient<DetailViewModel>()
                .BuildServiceProvider());

    }
}