using Microsoft.AspNetCore.SignalR;
using BackgammonLogic;
using Newtonsoft.Json;
using Entities;

namespace Network.Services.Server
{
    public class GameHub : Hub
    {
        NetGame game;
        Dictionary<string, List<string>> rooms;
        GameHub()
            => game = new NetGame();

        public async Task MakeRoomRequest(string roomName)
        {
            bool dontExist = !rooms.ContainsKey(roomName);
            string message;
            bool response = false;
            if (dontExist)
            {
                try
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    rooms[roomName] = new List<string>() { Context.ConnectionId };
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
                if (rooms[roomName].Count == 2)
                {
                    try
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                        rooms[roomName].Add(Context.ConnectionId);
                        message = "Room joined successfully";
                        response = true;
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

            await Clients.Caller.SendAsync("MakeRoomAnswer", response, message);
        }
        public async Task LeaveRoom()
        {
            string roomName = RoomNameByUserId(Context.UserIdentifier);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            rooms[roomName].Remove(Context.UserIdentifier);

            if (rooms[roomName].Count == 0)
                rooms.Remove(roomName);
        }
        public async Task MoveRequest(int destination, int source)
        {
            game.Move(destination, source);
        }
        public async Task SendGameStatus()
        {
            var response = game.GetGameStatus();
            try
            {
                await Clients.Group(RoomNameByUserId(Context.ConnectionId))
                    .SendAsync("ReceiveGameStatus", response); //комнатные
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        string? RoomNameByUserId(string userId)
            => rooms.Keys.FirstOrDefault(key => rooms[key].Contains(userId));
    }

}
    