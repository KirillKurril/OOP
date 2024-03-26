using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Network.Services.Client
{
    public class ChatClient
    {
        private readonly string _url;
        private HubConnection _hubConnection;

        public ChatClient(string url)
        {
            _url = url;
        }

        public async Task StartAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_url)
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            await _hubConnection.StartAsync();

            Console.WriteLine("Ваше имя?");
            var name = Console.ReadLine();

            while (true)
            {
                Console.Write("---> ");
                var message = Console.ReadLine();
                await _hubConnection.InvokeAsync("SendMessage", message, name);
            }
        }
    }
}
