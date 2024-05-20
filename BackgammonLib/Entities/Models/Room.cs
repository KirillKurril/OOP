using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.GameServices;

namespace ServerDB.Models
{
    public class Room
    {
        public string Id { get; set; }
        public List<string> Players { get; set; } = [];
        public NetGame NetGame { get; set; } = null!;
        public Room(string Id)
        {
            this.Id = Id;
            Players = new List<string>();
            NetGame = new NetGame();
            NetGame.
        }
    }
}
