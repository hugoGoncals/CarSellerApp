using CarSellerApp.Services;
using CarSellerApp.ViewModels;
using CarSellerCore.Services;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Dashboard;
using CarSellerCore.ViewModel.Details;
using CarSellerCore.ViewModel.Filter;
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
                .AddSingleton<IDialogService, DialogService>()
                .AddTransient<DashboardViewModel, AndroidDashboardViewModel>()
                .AddTransient<FilterViewModel, AndroidFilterViewModel>()
                .AddTransient<DetailViewModel>()
                .BuildServiceProvider());

    }
}