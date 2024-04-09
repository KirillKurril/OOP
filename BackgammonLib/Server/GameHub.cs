using Microsoft.AspNetCore.SignalR;
using BackgammonLogic;
using Newtonsoft.Json;
using Entities;

namespace Network.Services.Server
{
    public class GameHub : Hub
    {
        Dictionary<string, Room> rooms;
        public GameHub()
            => rooms = new Dictionary<string, Room>();
        public async Task CreateRoomRequest(string roomName)
        {
            bool dontExist = !rooms.ContainsKey(roomName);
            string message;
            bool response = false;
            if (dontExist)
            {
                try
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    rooms[roomName] = new Room(Context.ConnectionId);
                    message = "Room created successfully";
                    response = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    message = $"Error: {ex.Message}";
                }
            }
            else
                message = "Room already exists!";

            await Clients.Caller.SendAsync("MakeRoomAnswer", response, message);
        }
        public async Task JoinRoomRequest(string roomName)
        {
            string message;
            bool response = false;

            if (rooms.ContainsKey(roomName))
            {
                if (rooms[roomName].Players.Count == 2)
                {
                    try
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                        rooms[roomName].Add(Context.ConnectionId);
                        message = "Room joined successfully";
                        response = true;
                        await Clients.Group(roomName)
                            .SendAsync("RoomCompleted");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        message = $"Error: {ex.Message}";
                    }
                }
                else
                    message = "Room's full!";
            }
            else
                message = "Room doesn't exists!";

            await Clients.Caller.SendAsync("CreateRoomAnswer", response, message);
        }
        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            rooms[roomName].Remove(roomName);
            if (rooms[roomName].Players.Count == 0)
                rooms.Remove(roomName);
        }
        public void MoveRequest(int destination, int source, string roomName)
        {
            Task.Run(() =>
            {
                rooms[roomName].Game.Move(destination, source);
                SendGameStatus(roomName);
            });

        }
        public async Task SendGameStatus(string roomName)
        {
            var response = rooms[roomName].Game.GetGameStatus();
            try
            {
                await Clients.Group(roomName)
                    .SendAsync("ReceiveGameStatus", response); //комнатные
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    public class Room
    {
        public string Name { get; set; }
        public List<string> Players { get; set; }
        public NetGame Game { get; set; }
        public int ColorCounter { get; set; }
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
            Name = string.Empty;
            Players = new List<string>() { userConnectionId };
            Game = new NetGame();
        }
    };
}
    