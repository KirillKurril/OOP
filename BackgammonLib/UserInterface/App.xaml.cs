using System.Windows;
using UserInterface;

public partial class App : Application
{
    private readonly IHost _host;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public App()
    {
        _host = Host.CreateDefaultBuilder()
        .ConfigureServices((services) =>
        {
            services.AddSingleton<mainwindow>();
        })
        .Build();
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        MainWindow = _host.Services.GetRequiredService<mainwindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}