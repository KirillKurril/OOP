using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserInterface
{
    public partial class MenuPage : Page
    {
        private readonly INavigationService _navigationService;
        public MenuPage(INavigationService navigationService)
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
