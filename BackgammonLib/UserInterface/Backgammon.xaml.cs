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
using BackgammonLogic;

namespace UserInterface
{
    /// <summary>
    /// Логика взаимодействия для Backgammon.xaml
    /// </summary>
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
            (int, int)[] positionsInfo = game.GetDetailedReport();
            for(int i = 0; i < 24; ++i)
            {
                StackPanel stackPanel = (StackPanel)FindName($"S{i}");
                stackPanel.Children.Clear();
                for(int j = 0; j < positionsInfo[i].Item2; ++j)
                {
                    Ellipse piece = new Ellipse();
                    piece.Width = 20;
                    piece.Height = 20;

                    if (positionsInfo[i].Item1 == 0)
                        piece.Fill = Brushes.White;
                    else
                        piece.Fill = Brushes.Black;

                    stackPanel.Children.Add(piece);
                }
            }

        }

        private void PositionSelected(object sender, RoutedEventArgs e)
        {
            int position = Convert.ToInt32(((Button)sender).Name.Substring(1));
            int[] fieldForce = game.Status();
            if (firstChosenPosition == -1)
            {
                if (fieldForce[position] == game.curPlayer.Color)
                    firstChosenPosition = position;
                if (game.ReachedHome())
                {
                    Button throwButton = (Button)FindName("Throw");
                    throwButton.Visibility = Visibility.Visible;
                    throwButton.IsEnabled = true;
                }
            }
            else
            {
                if (!game.MovsAvalible())
                {
                    game.NewTurn();
                    return;
                    //сообщение о следующем ходе
                }
                    
                if (position == 24)
                    game.Move(firstChosenPosition, position);
                if (fieldForce[position] == -1)
                    game.Move(firstChosenPosition, position);
                if (game.curPlayer.Color == 0 && game.moveValues.Contains(position - firstChosenPosition)
                    || game.curPlayer.Color == 1 && game.moveValues.Contains(firstChosenPosition - position))
                    game.Move(firstChosenPosition, position);
                else if (fieldForce[position] == game.curPlayer.Color)
                    firstChosenPosition = position;
                else return;
                
                if(firstChosenPosition != position)
                {
                    Refresh();
                    firstChosenPosition = -1;
                }

            }
        }
    }
}
