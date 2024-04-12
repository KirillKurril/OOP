using BackgammonLogic;
using System.ComponentModel.DataAnnotations;

namespace ServerDB.Models
{
    public class Room
    {
        public Room(string ID)
        {
            this.ID = ID;
            Players = new List<string> ();
            Game = new NetGame();
        }
        [Key]
        public string ID { get; set; }
        public List<string> Players { get; set; }
        public NetGame Game { get; set; }
    }
}
