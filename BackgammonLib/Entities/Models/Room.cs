using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.GameServices;
using Entities.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerDB.Models
{
    public class Room
    {
        public string Id { get; set; }
        public List<string> Players { get; set; } = null!;
        public NetGame NetGame { get; set; } = null!;
        public int CountPlayersAssignedColor { get; set; }

        public Room() { }

        public static Room CreateRoom(string id)
        {
            Room room = new Room();
            room.Id = id;
            room.Players = new List<string>();
            room.NetGame = NetGame.CreateNetGame();
            room.CountPlayersAssignedColor = 0;
            return room;
        }
    }
}
