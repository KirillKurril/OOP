using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonEntities
{
    public class GameBoard
    {

        public List<Cell> whiteField;
        public List<Cell> blackField;

        public List<Piece>? pieсes;
        public int Length = 24;
        public List<Cell> WhiteField
        {
            get => whiteField;
            private set => whiteField = value;
        }
        public List<Cell> BlackField
        {
            get => blackField;
            private set => blackField = value;
        }
        public List<Piece>? Pieces
        {
            get => pieсes;
            private set => pieсes = value;
        }
        
        public GameBoard()
        {
            Pieces = new List<Piece>();

            whiteField = new List<Cell>();
            for (int i = 0; i < 24; ++i)
            {
                Cell cell = new Cell();
                whiteField.Add(cell);
            }

            blackField = new List<Cell>();
            for (int i = 12; i < 24; ++i)
                blackField.Add(WhiteField[i]);
            for (int i = 0; i < 12; ++i)
                blackField.Add(WhiteField[i]);

            for (int i = 0; i < 15; ++i)
            {
                Piece whitePiece = new Piece(Colors.White());
                Piece blackPiece = new Piece(Colors.Black());
                Pieces.Add(whitePiece);
                Pieces.Add(blackPiece);
                whiteField[0].Push(whitePiece);
                whiteField[12].Push(blackPiece);
            }
        }
    }
}
