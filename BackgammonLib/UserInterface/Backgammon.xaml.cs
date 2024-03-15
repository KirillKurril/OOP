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
    public partial class Backgammon : Page
    {
        private Game game;
        int firstChosenPosition;
        public Backgammon()
        {
            InitializeComponent();
            game = new Game();
            firstChosenPosition = -1;
            game.NewTurn();
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

                    if (positionsInfo[i].Item1 == 1)
                        piece.Fill = Brushes.White;
                    else
                        piece.Fill = Brushes.Black;

                    stackPanel.Children.Add(piece);
                }
            }

        }
        private void PositionSelected(object sender, RoutedEventArgs e)
        {
            int position = -1;
            try
            {
                if (((Button)sender).Name == "Throw")
                    position = -1;
            }
            catch 
            {
                position = Convert.ToInt32(((StackPanel)sender).Name.Substring(1));
            }

            if (game.MovsAvalibleExist() && game.GetStatus()[position] == game.GetPlayerColor())
            {
                if (firstChosenPosition == -1)  //если загнаны в последний дом появляет кнопку для вывода 
                {
                    firstChosenPosition = position;

                    if (game.GetPlayerStatus())
                    {
                        Throw.Visibility = Visibility.Visible;
                        Throw.IsEnabled = true;
                    }
                }
                else
                {
                    if(game.MoveConfirm(firstChosenPosition, position))
                        game.Move(firstChosenPosition, position);
                }

            }
            game.NewTurn();//отобразить что на кубиках выпало
        }
    }
}

