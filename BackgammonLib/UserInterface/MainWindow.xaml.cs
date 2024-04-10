using System.Windows;

namespace UserInterface
{
    public partial class MainWindow : Window
    {
        private readonly INavigationService _navigationService;
        public MainWindow(INavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _navigationService.SetFrame(MainFrame);
            _navigationService.NavigateToMenu();
        }
      
    }
}