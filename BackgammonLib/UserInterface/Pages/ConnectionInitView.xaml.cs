using Network.Interfaces;
using Network.Services.Client;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace UserInterface.Pages
{
    public partial class ConnectionInitView : Page
    {
        private readonly CreateSingerViewModel _viewModel;
        public CreateSingerView(CreateSingerViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
        public ConnectionInitView(IClient client, INavigationService navigationService)
        {
            InitializeComponent();
            _client = client;
            _navigationService = navigationService;
            _client.SetURL("https://localhost:7250/game");
            _client.ConnectionStatusEvent += ConnectionStatus;
            _client.CreateRoomResponseEvent += RoomConnectionHandler;
            _client.JoinRoomResponseEvent += RoomConnectionHandler;
            Task.Run(() => _client.Connect());
        }
    }
}
