using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
        MoveVerifier verifier;

        int firstPosition = -1;
        bool throwEnabled = false;
        bool fieldEnabled = false;

        public OnlineGame()
        {
            InitializeComponent();
            client.ReceiveGameStatusEvent += ReceiveGameData;
            client.EndGame += EndGame;
        }

        void ConnectButtonClicked(object sender, RoutedEventArgs e)
            =>  Task.Run(async () => await client.Connect());

        private void PositionSelected(object sender, RoutedEventArgs e)
        {
            int position;
            if (((Button)sender).Name == "Throw")
                position = 25;
            else
            {
                position = Convert.ToInt32(((Button)sender).Name.Substring(1));
                if (verifier.Color == Entities.Colors.Black())
                    position = (position + 12) % 24;
            }

            if (firstPosition != -1 
                && verifier.MoveConfirm(firstPosition, position))
            {
                Task.Run(async () 
                    => await client.MoveRequest(firstPosition, position));
                firstPosition = -1;
            }
        }

        private void CancelChoiсe(object sender, MouseButtonEventArgs e)
            => firstPosition = -1;


        private void EndGame()
        {
            MessageBox.Show($"Congratulations!\n{((verifier.Color == Entities.Colors.White()) ?
                ("White") : ("Black"))}" +
                $" player win!");
            //перезагрузить страницу
        }
        private void ReceiveColor(object sender, int color)
            => verifier.Color = color;
        public void ReceiveGameData(object sender, GameStatusData data)
        {
            ///
        }
        
        public void EndGameHandler(object sender, EventArgs eventArgs)
        {
            ///
        }

        void RefreshUi((int, int) ExtraStatus)
        {
            ///
        }

        ///метод закрытия страницы который отключается от сервера
    }
}

