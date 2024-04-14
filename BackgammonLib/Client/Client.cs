using System;
using System.Drawing;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Network.Interfaces;
using Newtonsoft.Json;

namespace Network.Services.Client
{
    public class Client : IClient
    {
        public string _roomName { get; private set; }
        public string URL { get; private set; }
        public HubConnection hubConnection {  get; private set; }

        public delegate void ReceiveGameStatusDelegate(object sender, GameStatusData data);
        public delegate void CreateRoomResponseDelegate(object sender, bool answer, string message);
        public delegate void ConnectionStatusDelegate(object sender, string status);

        public event IClient.ReceiveGameStatusDelegate ReceiveGameStatusEvent;
        public event IClient.CreateRoomResponseDelegate CreateRoomResponseEvent;
        public event IClient.CreateRoomResponseDelegate JoinRoomResponseEvent;
        public event IClient.ConnectionStatusDelegate ConnectionStatusEvent;
        public event EventHandler RoomCompleted;
        public event EventHandler EndGame;
        public event EventHandler<int> ColorResponse;
        public Client() { }
        public void SetURL(string url)
            => URL = url;

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
                    ConnectionStatusEvent?.Invoke(this, "Ошибка подключения");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }

            hubConnection.On<string>("Test", (amongus) =>
            {
                File.WriteAllText("test.txt", amongus);
            });

            hubConnection.On<string>("GameStatusHandler", (amongus) =>
            {
                var data = JsonConvert.DeserializeObject<GameStatusData>(amongus);
                File.WriteAllText("test.txt", data.ToString());
                ReceiveGameStatusEvent?.Invoke(this, data);

            });

            hubConnection.On<string>("ReceiveGameStatus", (json) =>
            {
                var data = JsonConvert.DeserializeObject<GameStatusData>(json);
                File.WriteAllText("test0.txt", data.ToString());
                ReceiveGameStatusEvent?.Invoke(this, data);
            });
 

            hubConnection.On<bool, string>("CreateRoomAnswer", (bool createdSuccessfully, string message) =>
            {
                CreateRoomResponseEvent?.Invoke(this, createdSuccessfully, message);
            });

            hubConnection.On<bool, string>("JoinRoomAnswer", (bool joinedSuccessfully, string message) =>
            {
                JoinRoomResponseEvent?.Invoke(this, joinedSuccessfully, message);
            });

            hubConnection.On<string>("RoomCompleted", (string roomName) =>
            {
                _roomName = roomName;
                RoomCompleted?.Invoke(this, EventArgs.Empty);
            });

            hubConnection.On("EndGame", () =>
            {
                EndGame?.Invoke(this, EventArgs.Empty);
            });

            hubConnection.On("ColorResponse", (int color) =>
            {
                ColorResponse?.Invoke(this, color);
            });
        }
        public async Task MoveRequest(int source, int destination)
        {
            await hubConnection.InvokeAsync("MoveRequest", source, destination, _roomName);
        }
        public async Task CreateRoom(string roomName)
        {
            await hubConnection.InvokeAsync("CreateRoomRequest", roomName);
        }
        public async Task JoinRoom(string roomName)
        {
            await hubConnection.InvokeAsync("JoinRoomRequest", roomName);
        }
        public async Task LeaveRoom()
        {
            await hubConnection.InvokeAsync("LeaveRoom", _roomName);
        }
        public async Task RequestColor()
            => await hubConnection.InvokeAsync("ColorRequest", _roomName);
        public async Task Disconnect()
        {
            await hubConnection.StopAsync();
        }
    }
}
