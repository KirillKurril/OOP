using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerLogic.Interfaces;
using ServerLogic.Services;

namespace Network.Services
{
    public class Server
    {
        private IGame game;
        private List<TcpClient> clients;
        private TcpListener listener;
        public Server() 
        {
            game = new OnlineGame();
            clients = new List<TcpClient>();    
        }
        public async Task StartServerAsync(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            listener = new TcpListener(ip, port);
            listener.Start();
            Console.WriteLine($"Server started on {ip}:{port}");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                ProcessClientAsync(client);
            }
        }
    }
}
