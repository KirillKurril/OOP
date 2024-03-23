using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic.Interfaces;
using ServerLogic.Services;

namespace Network.Entities
{
    public class Server
    {
        private IGame game;
        public Server() 
        {
            game = new OnlineGame();
        }
    }
}
