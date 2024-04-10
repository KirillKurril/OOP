using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using UserInterface;
using System.Windows;
using System.Windows.Markup;
using Network.Services.Client;
using Network.Interfaces;

namespace UserInterface
{
    public partial class App : Application
    {
        public static IHost? AppHost{ get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<IClient, Client>();
                services.AddTransient<BackgammonPage>();
                services.AddTransient<OnlineGamePage>();
                services.AddTransient<ConnectionInit>();
                services.AddTransient<MenuPage>();

                services.AddSingleton<INavigationService>((sp) =>
                {
                    var serviceProvider = sp.GetService<IServiceProvider>();
                    return new NavigationService(serviceProvider);
                });
            })
            .Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();

            base.OnExit(e);
        }
    }

}