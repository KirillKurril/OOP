﻿using System;
using System.Drawing;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.SignalR.Client;
using Network.Interfaces;

namespace Network.Services.Client
{
    public class Client : IClient
    {
        public string URL;
        public HubConnection hubConnection;

        public delegate void ReceiveGameStatusDelegate(object sender, GameStatusData data);
        public delegate void CreateRoomResponseDelegate(object sender, bool answer, string message);
        public delegate void ConnectionStatusDelegate(object sender, string status);

        public event IClient.ReceiveGameStatusDelegate ReceiveGameStatusEvent;
        public event IClient.CreateRoomResponseDelegate CreateRoomResponseEvent;
        public event IClient.CreateRoomResponseDelegate JoinRoomResponseEvent;
        public event IClient.ConnectionStatusDelegate ConnectionStatusEvent;
        public event EventHandler RoomComplete;
        public event EventHandler EndGame;
        public event EventHandler<int> ReceiveColor;
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
                    ConnectionStatusEvent?.Invoke(this, "Ошибка сраки");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }



            hubConnection.On<GameStatusData>("ReceiveGameStatus", (GameStatusData data) =>
            {
                ReceiveGameStatusEvent?.Invoke(this, data); 
            });

            hubConnection.On<bool, string>("CreateRoomAnswer", (bool createdSuccessfully, string message) =>
            {
                CreateRoomResponseEvent?.Invoke(this, createdSuccessfully, message);
            });

            hubConnection.On<bool, string>("JoinRoomAnswer", (bool joinedSuccessfully, string message) =>
            {
                CreateRoomResponseEvent?.Invoke(this, joinedSuccessfully, message);
            });

            hubConnection.On("RoomCompleted", () =>
            {
                RoomComplete?.Invoke(this, EventArgs.Empty);
            });

            hubConnection.On("EndGame", () =>
            {
                EndGame?.Invoke(this, EventArgs.Empty);
            });

            hubConnection.On("ReceiveColor", (int color) =>
            {
                ReceiveColor?.Invoke(this, color);
            });
        }


        public async Task MoveRequest(int source, int destination)
        {
            await hubConnection.InvokeAsync("MoveRequest", source, destination);
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
            await hubConnection.InvokeAsync("LeaveRoom");
        }
        public async Task Disconnect()
        {
            await hubConnection.StopAsync();
        }
    }
}
