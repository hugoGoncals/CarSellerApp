using CarSellerCore.Services;
using CarSellerCore.Services.Abstraction;
using CarSellerCore.ViewModel.Dashboard;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CarSellerApp;

public static class DIContainer
{
    public static ServiceProvider Services { get; private set; }

    public static void Configure()
    {
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
                .AddTransient<DashboardViewModel>() // Register your ViewModel
                .AddSingleton<ICarService, CarService>()
                .BuildServiceProvider());
    }
}