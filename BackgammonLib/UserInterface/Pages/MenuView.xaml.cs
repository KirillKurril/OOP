using System.Windows;
using System.Windows.Controls;

namespace UserInterface.Pages
{
    public partial class MenuView : Page
    {
        private readonly INavigationService _navigationService;
        public MenuView(INavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
        }
        public void GoToBackgammon(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateToBackgammonPage();
        }
        public void GoToNetworkGame(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateToConnectionPage();
        }
    }
}
