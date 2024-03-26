using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            
            .ConfigureServices(services =>
            {
                services.AddSingleton<App>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
        
        var app = host.Services.GetService<App>();
        app?.Run();


    }
}