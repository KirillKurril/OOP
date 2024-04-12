using BackgammonLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDB.Models
{
    public class Room
    {
        public Room(string ID)
            =>  this.ID = ID;

        public string ID { get; set; }
        public List<string> Players { get; set; }
        public NetGame Game { get; set; }
    }
}
