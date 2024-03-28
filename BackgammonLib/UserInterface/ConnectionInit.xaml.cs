using Network.Interfaces;
using Network.Services.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
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
    /// <summary>
    /// Interaction logic for ConnectionInit.xaml
    /// </summary>
    public partial class ConnectionInit : Page
    {
        IClient client;
        public ObservableCollection<object> DialogWindowElements { get; set; }
        public ConnectionInit(IClient client)
        {
            DialogWindowElements = new ObservableCollection<object>();
            InitializeComponent();
            this.client = client;
            this.client.SetURL("https://localhost:7250/game");
            this.client.ConnectionStatusEvent += ConnectionStatus;
            this.client.CreateRoomResponseEvent += RoomConnectionHandler;
            Task.Run(() => client.Connect());
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
            => Task.Run(() => 
            {
                var textBox = (TextBox)FindName("dialogTextBox");
                var roomName = textBox.Text;
                client.CreateRoom(roomName);
            });

        public void JoinRoom(object sender, RoutedEventArgs e)
            => Task.Run(() => 
            {
                var textBox = (TextBox)FindName("dialogTextBox");
                var roomName = textBox.Text;
                client.JoinRoom(roomName);
            });
        private ObservableCollection<object> BasicDialogCollection()
        {

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
