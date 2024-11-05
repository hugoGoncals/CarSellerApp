using CarSellerCore.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace CarSellerApp;

public static class DIContainer
{
    public static ServiceProvider Services { get; private set; }

    public static void Configure()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<BaseViewModel>();
        Services = serviceCollection.BuildServiceProvider();
    }
}