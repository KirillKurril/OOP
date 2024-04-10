using System.Windows.Controls;

public interface INavigationService
{
    void SetFrame(Frame frame);
    void NavigateToBackgammonPage();
    void NavigateToOnlineGamePage();
    void NavigateToMenu();
    void NavigateToConnectionPage();

}