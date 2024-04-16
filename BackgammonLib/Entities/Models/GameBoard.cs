using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.GameServices;

namespace Entities.Models
{

    public class GameBoard
    {
        public int Id { get; set; }
        public List<Cell> WhiteField { get; set; } = [];
        public int NetGameId { get; set; }
        public NetGame NetGame { get; set; } = null!;

        [NotMapped]
        public List<Cell> BlackField
        {
            get
            {
                var blackField = new List<Cell>();
                for (int i = 12; i< 24; ++i)
                    blackField.Add(WhiteField[i]);
                for (int i = 0; i< 12; ++i)
                    blackField.Add(WhiteField[i]);
                return blackField;
            }
        }
        public GameBoard()
        {
            WhiteField = new List<Cell>();
            for (int i = 0; i < 24; ++i)
            {
                Cell cell = new Cell();
                WhiteField.Add(cell);
            }

            for (int i = 0; i < 15; ++i)
            {
                WhiteField[0].Push(Colors.WhitePiece);
                WhiteField[12].Push(Colors.BlackPiece);
            }
        }
    }
}
