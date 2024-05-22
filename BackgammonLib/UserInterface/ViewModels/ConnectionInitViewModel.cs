using CommunityToolkit.Mvvm.ComponentModel;
using Network.Interfaces;
using System.Windows.Controls;
using System.Windows;

namespace UserInterface.ViewModels
{
    public partial class ConnectionInitViewModel : ObservableObject
    {
        IClient _client;
        INavigationService _navigationService;

        private string _myProperty;

        public string MyProperty
        {
            get => _myProperty;
            set => SetProperty(ref _myProperty, value); 
        }

 

        public Visibility �reateOrJoinVisibility { get; set; } = Visibility.Visible;
        public bool �reateOrJoinEnabled { get; set; } = true;
        public Visibility �reateVisibility { get; set; } = Visibility.Collapsed;
        public bool �reateEnabled { get; set; } = false;
        public Visibility JoinVisibility { get; set; } = Visibility.Collapsed;
        public bool JoinEnabled { get; set; } = false;
        public Visibility BackaAndTextBoxVisibility { get; set; } = Visibility.Collapsed;
        public bool BackaAndTextBoxEnabled { get; set; } = false;
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
            �reateOrJoinVisibility = Visibility.Collapsed;
            �reateOrJoinEnabled = false;
            JoinVisibility = Visibility.Visible;
            JoinEnabled = true;
            BackaAndTextBoxVisibility = Visibility.Visible;
            BackaAndTextBoxEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            �reateOrJoinVisibility = Visibility.Visible;
            �reateOrJoinEnabled = true;
            JoinVisibility = Visibility.Collapsed;
            JoinEnabled = false;
            �reateVisibility = Visibility.Collapsed;
            �reateEnabled = false;
            BackaAndTextBoxVisibility = Visibility.Collapsed;
            BackaAndTextBoxEnabled = false;
        }

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            �reateOrJoinVisibility = Visibility.Collapsed;
            �reateOrJoinEnabled = false;
            �reateVisibility = Visibility.Visible;
            �reateEnabled = true;
            BackaAndTextBoxVisibility = Visibility.Visible;
            BackaAndTextBoxEnabled = true;
        }
    }
}