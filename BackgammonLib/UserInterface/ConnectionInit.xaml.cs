using Network.Services.Client;
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
    /// <summary>
    /// Interaction logic for ConnectionInit.xaml
    /// </summary>
    public partial class ConnectionInit : Page
    {
        Client client;
        public ConnectionInit()
        {
            InitializeComponent();
            InitializeAsync().Wait();
            client = new Client("https://localhost:7250");
            client.ConnectionStatusEvent += ConnectionStatus;
        }

        private async Task InitializeAsync()
        {
            // Асинхронные операции
            await Task.Delay(1000);
            Console.WriteLine("Initialization complete");
        }


        public void CreateRoom(object sender, RoutedEventArgs e)
        {
            DialogWindow.Children.Clear();
            var textBox = new TextBox();
            textBox.Style = (Style)FindResource("RoomName");
            Grid.SetRow(textBox, 0);
            DialogWindow.Children.Add(textBox);

            var ConfimButton = new Button();
            ConfimButton.Style = (Style)FindResource("DialogButton");
            ConfimButton.Content = "Подтвердить";
            Grid.SetRow(ConfimButton, 1);
            DialogWindow.Children.Add(ConfimButton);

            //await client.Connect();
        }
        public void JoinRoom(object sender, RoutedEventArgs e)
        {
            DialogWindow.Children.Clear();
            var textBox = new TextBox();
            textBox.Style = (Style)FindResource("RoomName");
            Grid.SetRow(textBox, 0);
            DialogWindow.Children.Add(textBox);

            var ConfimButton = new Button();
            ConfimButton.Style = (Style)FindResource("DialogButton");
            ConfimButton.Content = "Подтвердить";
            Grid.SetRow(ConfimButton, 1);
            DialogWindow.Children.Add(ConfimButton);
        }

        private void ConnectionStatus(object sender, string message)
        {
            MessageBox.Show(message);
        }
    }
}
