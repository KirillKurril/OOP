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

namespace UserInterface
{
    public partial class Backgammon : Page
    {
        private Game game;
        int firstChosenPosition;
        public Backgammon()
        {
            InitializeComponent();
            game = new Game();
            firstChosenPosition = -1;
            HideThrowButton();
            Refresh();
        }
        private void Refresh()
        {
            List<(int, int)> positionsInfo = game.GetDetailedReport();
            for(int i = 0; i < 24; ++i)
            {
                StackPanel stackPanel = (StackPanel)FindName($"S{i}");
                stackPanel.Children.Clear();
                for(int j = 0; j < positionsInfo[i].Item2; ++j)
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

            try
            {
                var diceValues = game.GetDiceValues();

                Image dice1 = (Image)FindName("DiceOneImage");
                if (dice1 != null)
                {
                    string firstDiceImagePath = $"pack://application:,,,/Sprites/dices/dice{diceValues[0]}.png";
                    Uri imageUri = new Uri(firstDiceImagePath, UriKind.Absolute);
                    dice1.Source = new BitmapImage(imageUri);
                }

                Image dice2 = (Image)FindName("DiceTwoImage");
                if (dice2 != null && diceValues.Count > 1)
                {
                    string secondDiceImagePath = $"pack://application:,,,/Sprites/dices/dice{diceValues[1]}.png";
                    Uri imageUri = new Uri(secondDiceImagePath, UriKind.Absolute);
                    dice2.Source = new BitmapImage(imageUri);
                }
                else if (dice2 != null)
                {
                    dice2.Source = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting image source: {ex.Message}");
            }

            Label whiteScoreLabel = (Label)FindName("WhiteScoreLabel");
            whiteScoreLabel.Content = game.GetScore().Item1.ToString();

            Label blackScoreLabel = (Label)FindName("BlackScoreLabel");
            blackScoreLabel.Content = game.GetScore().Item2.ToString();

            Ellipse currentMoveIndicator = (Ellipse)FindName("CurrentMoveIndicator");
            currentMoveIndicator.Fill = game.GetCurColor() == Entities.Colors.White() ? Brushes.White : Brushes.Black;

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
            string endgameMessage = 
                $"{((game.GetPlayerColor() == Entities.Colors.White()) ?
                ("Белый")
                : ("Черный"))}" +
                $" игрок победил!";
            CustomMessageBox messageBox = new CustomMessageBox(endgameMessage);
            messageBox.Closed += (sender, e) => NavigationService.GoBack();
            messageBox.Show();
        }
    }
}

