using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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

            {
                if (_rooms.Contains(roomName))
                    throw new Exception("Room already exists!");

                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                _rooms.Add(roomName);
                _rooms.AddPlayer(roomName, Context.ConnectionId);
                message = "Room created successfully";
                response = true;
            }

            await Clients.Caller.SendAsync("CreateRoomAnswer", response, message);
            await Task.Run(() => WriteLog(roomName, message));
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
                _rooms.AddPlayer(roomName, Context.ConnectionId);
                message = "Room joined successfully";
                response = true;
                WriteLog(roomName, $"{Context.ConnectionId} joined successfully");
                await Clients.Group(roomName).SendAsync("RoomCompleted", roomName);
                WriteLog(roomName, "RoomCompleted");
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            await Task.Run(() => WriteLog(roomName, message));
            await Clients.Caller.SendAsync("JoinRoomAnswer", response, message);
        }
        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            _rooms.Remove(roomName);
            if (_rooms.IsEmpty(roomName))
            {
                _rooms.Remove(roomName);
                await Task.Run(() => WriteLog(roomName, "Room deleted"));
            }
        }
        public async Task MoveRequest(int destination, int source, string roomName)
        {
                 _rooms.MakeMove(roomName, destination, source);
                await Task.Run(() => WriteLog(roomName,
                    $"User {Context.ConnectionId} requests for move {source} : {destination}"));
                await SendGameStatus(roomName);
        }
        public async Task SendGameStatus(string roomName)
        {
            var response = _rooms.GetStatus(roomName);
            //string response = JsonConvert.SerializeObject(data);
            try
            {
                await Clients.Group(roomName)
                    .SendAsync("ReceiveGameStatus", response);
                await Task.Run(() => WriteLog(roomName, "Status's sended"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task ColorRequest(string roomName)
        {
            Console.WriteLine($"\n\n\nПРИШЕЛ ЗАПРОС НА ЦВЕТ ОТ ИГРОКА: {Context.ConnectionId}\n\n\n");
            string response = string.Empty;
            try
            {
                var players = _rooms.GetPlayers(roomName);
                if (Context.ConnectionId == players[0])
                {
                    await Clients.Caller.SendAsync("ColorResponse", 1);
                }
                else
                {
                    await Clients.Caller.SendAsync("ColorResponse", -1);
                    await SendGameStatus(roomName);
                }
/*                {
                        var gameStat = _rooms.GetStatus(roomName);
                        Console.WriteLine(gameStat);

                        response = JsonConvert.SerializeObject(gameStat);
                        await Clients.Caller.SendAsync("GameStatusHandler", response);
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА ПРИ ОТПРАВКЕ ЦВЕТА {ex}");
                await Clients.Caller.SendAsync("ColorResponse", 0);
            }

            //await Clients.Caller.SendAsync("Test", response);
            
        }
        public void WriteLog(string roomName, string message)
        {
                string logMessage = $"{DateTime.Now} - {roomName}: {message}";
                Console.WriteLine(logMessage);


                Console.WriteLine("\nRooms:");
                foreach (var room in _rooms.GetRooms())
                {
                    Console.WriteLine($"Name: {room.Id}");
                    Console.WriteLine("Players:");
                    foreach(var player in room.Players)
                        Console.WriteLine($"\t + {player}");
                }
        }
    }
   
}
    