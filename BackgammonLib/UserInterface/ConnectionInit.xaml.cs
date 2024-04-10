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
        public ObservableCollection<object> DialogWindowElements { get; set; }
        public ConnectionInit(IClient client, INavigationService navigationService)
        {
            DialogWindowElements = new ObservableCollection<object>();
            DialogWindowElements = BasicDialogCollection();
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
        =>  DialogWindowElements = JoinDialogCollection();

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        =>  DialogWindowElements = CreateDialogCollection();
        private ObservableCollection<object> BasicDialogCollection()
        {
            var CreateRoomButton = new Button();
            CreateRoomButton.Content = "Создать комнату";
            CreateRoomButton.Style = (Style)FindResource("DialogButton");
            CreateRoomButton.Click += CreateRoomButton_Click;
            Grid.SetRow(CreateRoomButton, 0);

            var JoinRoomButton = new Button();
            JoinRoomButton.Content = "Присоединиться к комнате";
            JoinRoomButton.Style = (Style)FindResource("DialogButton");
            JoinRoomButton.Click += JoinRoomButton_Click;
            Grid.SetRow(JoinRoomButton, 1);

            var items = new ObservableCollection<object>() { CreateRoomButton, JoinRoomButton };

            return items;
        }

        private ObservableCollection<object> CreateDialogCollection()
        {
            var textBox = new TextBox();
            textBox.Name = "dialogTextBox";
            textBox.Style = (Style)FindResource("RoomName");
            Grid.SetRow(textBox, 0);

            var ConfimButton = new Button();
            ConfimButton.Style = (Style)FindResource("DialogButton");
            ConfimButton.Content = "Подтвердить";
            Grid.SetRow(ConfimButton, 1);
            ConfimButton.Click += CreateRoom;

            var items = new ObservableCollection<object>() {textBox, ConfimButton };

            return items;
        }
        private ObservableCollection<object> JoinDialogCollection()
        {
            var textBox = new TextBox();
            textBox.Style = (Style)FindResource("RoomName");
            Grid.SetRow(textBox, 0);

            var ConfimButton = new Button();
            ConfimButton.Style = (Style)FindResource("DialogButton");
            ConfimButton.Content = "Подтвердить";
            Grid.SetRow(ConfimButton, 1);
            ConfimButton.Click += JoinRoom;

            var items = new ObservableCollection<object>() { textBox, ConfimButton };

            return items;
        }
    }
}
