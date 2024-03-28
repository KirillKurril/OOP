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
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
            .ConfigureServices((services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<IClient, Client>();
            })
            .Build();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync().GetAwaiter().GetResult();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}