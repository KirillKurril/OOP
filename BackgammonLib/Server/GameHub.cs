using Microsoft.AspNetCore.SignalR;
using BackgammonLogic;
using Newtonsoft.Json;
using Entities;
using Microsoft.Extensions.Logging;
using System;
using ServerDB.Repositories;


namespace Network.Services.Server
{
    public class GameHub : Hub
    {
        IRoomRepository _rooms;
        public GameHub(IRoomRepository repository)
        {
            _rooms = repository;
        }

        public async Task CreateRoomRequest(string roomName)
        {
            string message ="";
            bool response = false;

            try
            {
                if (_rooms.Contains(roomName))
                    throw new Exception("Room already exists!");

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                _rooms.Add(Context.ConnectionId);
                message = "Room created successfully";
                response = true;
            }
            catch(Exception ex)
            {
                message = ex.Message;
            }

            await Clients.Caller.SendAsync("CreateRoomAnswer", response, message);
            Task.Run(() => WriteLog(roomName, message));
        }
        public async Task JoinRoomRequest(string roomName)
        {
            string message;
            bool response = false;
            try
            {
                if (!_rooms.Contains(roomName))
                    throw new Exception("Room doesn't exists!");

                if (_rooms.IsFull(roomName))
                    throw new Exception("Room's full!");

                if (_rooms.ContainsPlayer(roomName, Context.ConnectionId))
                    throw new Exception("You have already entered the room!");

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                _rooms.Add(Context.ConnectionId);
                message = "Room joined successfully";
                response = true;
                Task.Run(() => WriteLog(roomName, $"{Context.ConnectionId} joined successfully"));
               
                Task.Run(async () => {
                    await Clients.Group(roomName).SendAsync("RoomCompleted");
                    WriteLog(roomName, "RoomCompleted");
                }); 
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Task.Run(() => WriteLog(roomName, message));
            await Clients.Caller.SendAsync("JoinRoomAnswer", response, message);
        }
        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            _rooms.Remove(roomName);
            if (_rooms.IsEmpty(roomName))
            {
                _rooms.Remove(roomName);
                Task.Run(() => WriteLog(roomName, "Room deleted"));
            }
        }
        public void MoveRequest(int destination, int source, string roomName)
        {
            Task.Run(() =>
            {
                _rooms.MakeMove(roomName, destination, source);
                Task.Run(() => WriteLog(roomName,
                    $"User {Context.ConnectionId} requests for move {source} : {destination}"));
                SendGameStatus(roomName);
            });

        }
        public async Task SendGameStatus(string roomName)
        {
            var response = _rooms.GetStatus(roomName);
            try
            {
                await Clients.Group(roomName)
                    .SendAsync("ReceiveGameStatus", response);
                Task.Run(() => WriteLog(roomName, "Status's sended"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task ColorRequest(string roomName)
        {
            if (_rooms.GetCurColor(roomName) == 0)
                await Clients.Caller.SendAsync("ColorResponse", 1);
            else
                await Clients.Caller.SendAsync("ColorResponse", 1);
        }
        public void WriteLog(string roomName, string message)
        {
                string logMessage = $"{DateTime.Now} - {roomName}: {message}";
                Console.WriteLine(logMessage);


                Console.WriteLine("\nRooms:");
                foreach (var room in _rooms.GetRooms())
                {
                    Console.WriteLine($"Name: {room.ID}");
                    Console.WriteLine("Players:");
                    foreach(var player in room.Players)
                        Console.WriteLine($"\t + {player}");
                }
        }

        private class Room
        {
        }
    }
   
}
    