using System;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace Network.Services.Client
{
    public class Client
    {
        private string URL;
        private HubConnection hubConnection;
        HubEventHandler hubEventHandler;

        public class HubEventHandler
        {
            public event EventHandler<string> MessageReceived;

            public void OnMessageReceived(string message)
            {
                MessageReceived?.Invoke(this, message);
            }
        }

        public Client(string url)
        {
            URL = url;
            hubEventHandler = new MyHubEventHandler();
        }
        public async Task Connect()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(URL)
                .Build();
            await hubConnection.StartAsync();

            hubConnection.On<string>(string gameData) =>
            {
                hubEventHandler.OnMessageReceived(message);
            }
        }

        public async Task MoveRequest(string request)
        {
            await hubConnection.SendAsync("MoveRequest", request);
        }

        public async Task StartAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(URL)
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            await hubConnection.StartAsync();

            Console.WriteLine("Ваше имя?");
            var name = Console.ReadLine();

            while (true)
            {
                Console.Write("---> ");
                var message = Console.ReadLine();
                await hubConnection.InvokeAsync("SendMessage", message, name);
            }
        }

        public async Task SendMessageAsync(string message, string name)
        {
            await hubConnection.InvokeAsync("SendMessage", message, name);
        }

        public GameStatusData ReceiveGameStatus(string message)
        {

        }
    }
}
