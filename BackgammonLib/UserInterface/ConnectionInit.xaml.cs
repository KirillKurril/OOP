using Network.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace UserInterface
{
    public partial class ConnectionInit : Page
    {
        IClient _client;
        INavigationService _navigationService;
        
        public Visibility СreateOrJoinVisibility { get; set; } = Visibility.Visible;
        public bool СreateOrJoinEnabled { get; set; } = true;
        public Visibility СreateVisibility { get; set; } = Visibility.Collapsed;
        public bool СreateEnabled { get; set; } = false;
        public Visibility JoinVisibility { get; set; } = Visibility.Collapsed;
        public bool JoinEnabled { get; set; } = false;
        public Visibility BackaAndTextBoxVisibility { get; set; } = Visibility.Collapsed;
        public bool BackaAndTextBoxEnabled { get; set; } = false;
        public ConnectionInit(IClient client, INavigationService navigationService)
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

        private void ConnectionStatus(object sender, string message)
        {
            MessageBox.Show(message);
        }

        private void RoomConnectionHandler(object sender, bool answer, string message)
        {
            MessageBox.Show(message);
        }

        public void CreateRoom(object sender, RoutedEventArgs e)
            => Task.Run(async () => 
            {
                var textBox = (TextBox)FindName("dialogTextBox");
                var roomName = textBox.Text;
                await _client.CreateRoom(roomName);
            });

        public void JoinRoom(object sender, RoutedEventArgs e)
            => Task.Run(() => 
            {
                var textBox = (TextBox)FindName("dialogTextBox");
                var roomName = textBox.Text;
                _client.JoinRoom(roomName);
            });
       

        private void JoinRoomButton_Click(object sender, RoutedEventArgs e)
        {
            СreateOrJoinVisibility = Visibility.Collapsed;
            СreateOrJoinEnabled = false;
            JoinVisibility = Visibility.Visible;
            JoinEnabled = true;
            BackaAndTextBoxVisibility = Visibility.Visible;
            BackaAndTextBoxEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            СreateOrJoinVisibility = Visibility.Visible;
            СreateOrJoinEnabled = true;
            JoinVisibility = Visibility.Collapsed;
            JoinEnabled = false;
            СreateVisibility = Visibility.Collapsed;
            СreateEnabled = false;
            BackaAndTextBoxVisibility = Visibility.Collapsed;
            BackaAndTextBoxEnabled = false;
        }

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            СreateOrJoinVisibility = Visibility.Collapsed;
            СreateOrJoinEnabled = false;
            СreateVisibility = Visibility.Visible;
            СreateEnabled = true;
            BackaAndTextBoxVisibility = Visibility.Visible;
            BackaAndTextBoxEnabled = true;
        }
    }
}
