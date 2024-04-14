using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Entities.Models;
using Logic.Entities;
using Network.Interfaces;
using Network.Services.Client;

namespace UserInterface
{
    public partial class OnlineGamePage : Page
    {
        IClient _client;
        INavigationService _navigationService;
        MoveVerifier verifier;

        int firstPosition = -1;
        bool throwEnabled = false;
        bool fieldEnabled = false;
        int whiteScore = -1;
        int blackScore = -1;
        int moveColor = 0;

        public OnlineGamePage(INavigationService navigationService, IClient client)
        {
            InitializeComponent();
            _client = client;
            _navigationService = navigationService;
            _client.ColorResponse += ReceiveColorHandler;
            _client.ReceiveGameStatusEvent += ReceiveGameDataHandler;
            _client.EndGame += EndGameHandler;

            Unloaded += UnloadPageHandler;
        }

        private void PositionSelected(object sender, RoutedEventArgs e)
        {
            int position;
            if (((Button)sender).Name == "Throw")
                position = 25;
            else
            {
                position = Convert.ToInt32(((Button)sender).Name.Substring(1));
                if (verifier.Color == Entities.Models.Colors.Black())
                    position = (position + 12) % 24;
            }

            if (firstPosition != -1 
                && verifier.MoveConfirm(firstPosition, position))
            {
                Task.Run(() 
                    => _client.MoveRequest(firstPosition, position));
                firstPosition = -1;
                throwEnabled = false;
            }
            else if(verifier.MoveConfirm(firstPosition, position))
            {
                firstPosition = position;
                if (verifier.Throwable(position))
                    throwEnabled = true;
            }
        }

        private void CancelChoiсe(object sender, MouseButtonEventArgs e)
            => firstPosition = -1;
        private void ReceiveColorHandler(object sender, int color)
            => verifier.Color = color;
        public void ReceiveGameDataHandler(object sender, GameStatusData data)
        {
            verifier.Update(data.Status, data.DiceValues, data.MoveValues, data.ReachedHome, data.HatsOffToYou, data.Safemode);
            RefreshField(data.ExtraStatus);
            fieldEnabled = verifier.Color == data.MoveColor;
            whiteScore = data.Score.Item1;
            blackScore = data.Score.Item2;
            moveColor = data.MoveColor;
        }
        
        public void EndGameHandler(object sender, EventArgs e)
        {
            MessageBox.Show($"Congratulations!\n{((verifier.Color == Entities.Models.Colors.White()) ?
           ("White") : ("Black"))}" +
           $" player win!");

        }

        void RefreshField(List<(int, int)> extraStatus)
        {
            ///надо колдовать через биндинг
            for (int i = 0; i < 24; ++i)
            {
                StackPanel stackPanel = (StackPanel)FindName($"S{i}");
                stackPanel.Children.Clear();
                for (int j = 0; j < extraStatus[i].Item2; ++j)
                {
                    Ellipse piece = new Ellipse();
                    piece.Width = 20;
                    piece.Height = 20;

                    if (extraStatus[i].Item1 == 1)
                        piece.Fill = Brushes.White;
                    else
                        piece.Fill = Brushes.Black;

                    stackPanel.Children.Add(piece);
                }
            }
            ///надо колдовать через биндинг
        }

        protected void UnloadPageHandler(object sender, RoutedEventArgs e)
        {
            Task.Run(() => _client.Disconnect());
        }
    }
}

