using System;
using System.Collections.Generic;
using Entities.GameServices;

namespace Entities.Models
{

    public class GameBoard
    {
        public int Id { get; set; }
        public List<Cell> WhiteField { get; set; }
        public List<Cell> BlackField { get; set; }

        public int NetGameId { get; set; }
        public NetGame? NetGame { get; set; }
        public GameBoard()
        {
            WhiteField = new List<Cell>();
            for (int i = 0; i < 24; ++i)
            {
                Cell cell = new Cell();
                WhiteField.Add(cell);
            }

            BlackField = new List<Cell>();
            for (int i = 12; i < 24; ++i)
                BlackField.Add(WhiteField[i]);
            for (int i = 0; i < 12; ++i)
                BlackField.Add(WhiteField[i]);

            for (int i = 0; i < 15; ++i)
            {
                WhiteField[0].Push(Colors.WhitePiece);
                WhiteField[12].Push(Colors.BlackPiece);
            }
        }
    }
}
