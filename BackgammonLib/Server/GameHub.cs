using Microsoft.AspNetCore.SignalR;
using BackgammonLogic;
using Newtonsoft.Json;
using Entities;
using Microsoft.Extensions.Logging;
using System;

//Акак логгировать если хабов много?
//Создается много хабов 

namespace Network.Services.Server
{
    public class GameHub : Hub
    {
        static Dictionary<string, Room> _rooms;
        static string _logFile = "log.txt";
        static object _locker = new object();
        public GameHub()
        {
            _rooms = new Dictionary<string, Room>();
            //using (FileStream fileStream = new FileStream(_logFile, FileMode.Create)) { }
        }

        public async Task CreateRoomRequest(string roomName)
        {
            string message ="";
            bool response = false;

            try
            {
                if (_rooms.ContainsKey(roomName))
                    throw new Exception("Room already exists!");

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                _rooms[roomName] = new Room(Context.ConnectionId);
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
                if (!_rooms.ContainsKey(roomName))
                    throw new Exception("Room doesn't exists!");

                if (_rooms[roomName].Players.Count == 2)
                    throw new Exception("Room's full!");

                if (_rooms[roomName].Players.Contains(Context.ConnectionId))
                    throw new Exception("You have already entered the room!");

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                _rooms[roomName].Add(Context.ConnectionId);
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
            _rooms[roomName].Remove(roomName);
            if (_rooms[roomName].Players.Count == 0)
            {
                _rooms.Remove(roomName);
                Task.Run(() => WriteLog(roomName, "Room deleted"));
            }
        }
        public void MoveRequest(int destination, int source, string roomName)
        {
            Task.Run(() =>
            {
                _rooms[roomName].Game.Move(destination, source);
                Task.Run(() => WriteLog(roomName,
                    $"User {Context.ConnectionId} requests for move {source} : {destination}"));
                SendGameStatus(roomName);
            });

        }
        public async Task SendGameStatus(string roomName)
        {
            var response = _rooms[roomName].Game.GetGameStatus();
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
            if (_rooms[roomName].GetPlayerIndex(Context.ConnectionId) == 0)
                await Clients.Caller.SendAsync("ColorResponse", 1);
            else
                await Clients.Caller.SendAsync("ColorResponse", 1);
        }
        public void WriteLog(string roomName, string message)
        {
            lock (_locker)
            {
                string logMessage = $"{DateTime.Now} - {roomName}: {message}";
                using (StreamWriter writer = new StreamWriter(_logFile, true))
                {
                    //File.AppendAllText(_logFile, logMessage + Environment.NewLine);
                }
                Console.WriteLine(logMessage);


                Console.WriteLine("\nRooms:");
                foreach (var room in _rooms)
                {
                    Console.WriteLine($"Name: {room.Key}");
                    Console.WriteLine("Players:");
                    foreach(var player in room.Value.Players)
                        Console.WriteLine($"\t + {player}");
                }
            }
        }
    }
    public class Room
    {
        public List<string> Players { get; set; }
        public NetGame Game { get; set; }
        public void Add(string userConnectionId)
        {
            Players.Add(userConnectionId);
        }
        public void Remove(string userConnectionId)
        {
            Players.Remove(userConnectionId);
        }
        public Room(string userConnectionId)
        {
            Players = new List<string>() { userConnectionId };
            Game = new NetGame();
        }
        public int GetPlayerIndex(string userConnectionId)
            => Players.IndexOf(userConnectionId);
    };
}
    