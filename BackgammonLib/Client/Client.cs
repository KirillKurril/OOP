using System;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace Network.Services.Client
{
    public class Client
    {
        public string URL;
        public HubConnection hubConnection;

        public delegate GameStatusData ReceiveGameStatusDelegate(object sender, GameStatusData e);
        public event ReceiveGameStatusDelegate ReceiveGameStatusEvent;

        public delegate bool CreateRoomResponseDelegate(object sender, bool answer, string message);
        public event CreateRoomResponseDelegate CreateRoomResponseEvent;
        public event CreateRoomResponseDelegate JoinRoomResponseEvent;

        public delegate void ConnectionStatusDelegate(object sender, string status);
        public event ConnectionStatusDelegate ConnectionStatusEvent;    
        public Client(string url)
        {
            URL = url;
        }


        public async Task Connect()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(URL)
                .Build();
            try
            {
                await hubConnection.StartAsync();
                if (hubConnection.State == HubConnectionState.Connected)
                    ConnectionStatusEvent?.Invoke(this, "Подключение выполнено успешно");
                else
                    ConnectionStatusEvent?.Invoke(this, "Ошибка сраки");
            }
            catch (Exception ex)
            {
                // Обработка ошибки соединения
                Console.WriteLine($"Connection error: {ex.Message}");
            }



            hubConnection.On<GameStatusData>("ReceiveGameStatus", (GameStatusData data) =>
            {
                ReceiveGameStatusEvent?.Invoke(this, data); 
            });

            hubConnection.On<bool, string>("MakeRoomAnswer", (bool createdSuccessfully, string message) =>
            {
                CreateRoomResponseEvent?.Invoke(this, createdSuccessfully, message);
            });

            hubConnection.On<bool, string>("JoinRoomAnswer", (bool joinedSuccessfully, string message) =>
            {
                CreateRoomResponseEvent?.Invoke(this, joinedSuccessfully, message);
            });
        }


        public async Task MoveRequest(string request)
        {
            await hubConnection.InvokeAsync("MoveRequest", request);
        }

        public async Task MakeRoom(string roomName)
        {
            await hubConnection.InvokeAsync("MakeRoomRequest", roomName);
        }
        public async Task JoinRoom(string roomName)
        {
            await hubConnection.InvokeAsync("JoinRoomRequest", roomName);
        }
        public async Task LeaveRoom(string roomName)
        {
            await hubConnection.InvokeAsync("LeaveRoom");
        }
    }
}
