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
            stat.Text = string.Join(" ", game.GetDiceValues());

        }
        private void PositionSelected(object sender, RoutedEventArgs e)
        {
            int position;
            bool startingPositionSelected = firstChosenPosition != -1;

            if (((Button)sender).Name == "Throw")
                    position = -1;
            else
            {
                position = Convert.ToInt32(((Button)sender).Name.Substring(1));
                
                if (game.GetPlayerColor() == Entities.Colors.Black())
                    position = Math.Abs(position + 12) % 24;
            }

            if (startingPositionSelected)
            {
                if (game.MoveConfirm(firstChosenPosition, position))
                {
                    game.Move(firstChosenPosition, position);
                    firstChosenPosition = -1;
                    Refresh();

                    if (game.CheckEndGame())
                        EndGame();

                    if (Throw.IsEnabled)
                        HideThrowButton();

                    if (!game.MovsAvalibleExist())
                    {
                        game.NewTurn();
                        Refresh();
                    }
                }
            }
            else
            {
                if (game.VerifyStartPosition(position))
                {
                    firstChosenPosition = position;
                    if (game.GetPlayerStatus())
                        ShowThrowButton();
                }
            }
        }

        private void CancelChoiсe(object sender, MouseButtonEventArgs e)
        {
            firstChosenPosition = -1;
            //MessageBox.Show("Правая кнопка мыши нажата!");
        }
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
        private void EndGame() { }
    }
}

