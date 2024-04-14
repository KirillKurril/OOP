using System.ComponentModel.DataAnnotations;
using Entities.GameServices;

namespace ServerDB.Models
{
    public class Room
    {
        public Room(string Id)
        {
            this.Id = Id;
            Players = new List<string> ();
            Game = new NetGame();
        }
        public string Id { get; set; }
        public List<string> Players { get; set; }
        public NetGame Game { get; set; }
    }
}
