using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BackgammonLogic;
using Entities;
using Logic.Entities;
using Network.Services.Client;

namespace UserInterface
{
    public partial class OnlineGame : Page
    {
        Client client;
        MoveVerifier moveVerifier;
        bool connected;
        public OnlineGame()
        {
            connected = false;
            InitializeComponent();
            client = new Client();
            client.SetURL("https://localhost:7250/game");
            client.RoomComplete += RoomCompleteHandler;
            client.ConnectionStatusEvent += ConnectionStatus;
            client.CreateRoomResponseEvent += RoomConnectionHandler;
            client.JoinRoomResponseEvent += RoomConnectionHandler;
            //client.ReceiveGameStatusEvent +=

        }

        void ConnectButtonClicked(object sender, RoutedEventArgs e)
        {
            Task.Run(() => client.Connect());
        }

        private void Refresh()
        {
            List<(int, int)> positionsInfo = game.GetDetailedReport();
            for (int i = 0; i < 24; ++i)
            {
                StackPanel stackPanel = (StackPanel)FindName($"S{i}");
                stackPanel.Children.Clear();
                for (int j = 0; j < positionsInfo[i].Item2; ++j)
                {
                    Ellipse piece = new Ellipse();
                    piece.Width = 20;
                    piece.Height = 20;

                    if (positionsInfo[i].Item1 == 1)
                        piece.Fill = Brushes.White;
                    else
                        piece.Fill = Brushes.Black;

                    stackPanel.Children.Add(piece);
                }
            }
            stat.Text = string.Join(" ", game.GetDiceValues());
            stat.Text += $"\n\nwhite score:\n{game.GetScore().Item1}\n\nblack score:\n{game.GetScore().Item2}\n\nNow move\n{game.GetPlayerColor()}";

        }
        private void PositionSelected(object sender, RoutedEventArgs e)
        {
            int position;
            bool startingPositionSelected = firstChosenPosition != -1;

            if (((Button)sender).Name == "Throw")
                position = 25;
            else
            {
                position = Convert.ToInt32(((Button)sender).Name.Substring(1));

                if (game.GetPlayerColor() == Entities.Colors.Black())
                    position = (position + 12) % 24;
            }

            if (startingPositionSelected)
            {
                if (position == 25)
                {
                    game.Move(firstChosenPosition, position);
                    firstChosenPosition = -1;
                    Refresh();
                    HideThrowButton();
                }
                else if (game.MoveConfirm(firstChosenPosition, position))
                {
                    game.Move(firstChosenPosition, position);
                    firstChosenPosition = -1;
                    Refresh();
                    if (Throw.Visibility == Visibility.Visible)
                        HideThrowButton();
                }
                else
                    return;

                if (game.CheckEndGame())
                {
                    EndGame();
                    return;
                }

                if (!game.MovsAvalibleExist())
                {
                    game.NewTurn();
                    Refresh();
                }

            }
            else
            {
                if (game.VerifyStartPosition(position))
                {
                    firstChosenPosition = position;
                    if (game.GetPositionEctability(position))
                        ShowThrowButton();
                }
            }
        }

        private void CancelChoiсe(object sender, MouseButtonEventArgs e)
            => firstChosenPosition = -1;
        private void ShowThrowButton()
        {
            Throw.Visibility = Visibility.Visible;
            Throw.IsEnabled = true;
        }
        private void HideThrowButton()
        {
            Throw.Visibility = Visibility.Hidden;
            Throw.IsEnabled = false;
        }
        private void EndGame()
        {
            MessageBox.Show($"Congratulations!\n{((game.GetPlayerColor() == Entities.Colors.White()) ?
                ("White") : ("Black"))}" +
                $" player win!");
            game = new Game();
            firstChosenPosition = -1;
            Refresh();
        }
        ///////////////////////////////////////////////////////////////////////
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
                await client.CreateRoom(RoomName.Text);
            });
        public void JoinRoom(object sender, RoutedEventArgs e)
            => Task.Run(async () =>
            {
                await client.JoinRoom(RoomName.Text);
            });
        public void RoomCompleteHandler(object sender, EventArgs e)
        {
            MessageBox.Show("Room created successfully!");
        }
        public void Refresh
    }
}

