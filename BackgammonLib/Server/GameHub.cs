using Microsoft.AspNetCore.SignalR;
using BackgammonLogic;
using Newtonsoft.Json;
using Entities;

namespace Network.Services.Server
{
    public class GameHub : Hub
    {
        private NetGame game;

        GameHub()
            => game = new NetGame();   

        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task MoveRequest(string request)
        {
            (int destination, int source) = JsonConvert.DeserializeObject<(int, int)>(request);
            game.Move(destination, source);
        }
        public async Task SendGameStatus(GameStatusData data)
        {
            string message = JsonConvert.SerializeObject(data);
            await Clients.All.SendAsync("ReceiveGameStatus", data); //комнатные
        }
    }
}
