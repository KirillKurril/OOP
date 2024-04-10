using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UserInterface;

public class NavigationService : INavigationService
{
    private Frame _frame;
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public void SetFrame(Frame frame)
        => _frame = frame;

    public void NavigateToBackgammonPage()
    {
        var backgammonPage = _serviceProvider.GetService<BackgammonPage>();
        _frame.Navigate(backgammonPage);
    }

    public void NavigateToConnectionPage()
    {
        var connectionPage = _serviceProvider.GetService<ConnectionInit>();
        _frame.Navigate(connectionPage);
    }

    public void NavigateToOnlineGamePage()
    {
        var connectionPage = _serviceProvider.GetService<ConnectionInit>();
        _frame.Navigate(connectionPage);
    }

    public void NavigateToMenu()
    {
        var menuPage = _serviceProvider.GetService<MenuPage>(); // Предполагается, что у вас есть класс MenuPage для начальной страницы меню
        _frame.Navigate(menuPage);
    }
}