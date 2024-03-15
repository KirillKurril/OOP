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

        public Stack<Piece> [] field;
        public List<Piece>? pieсes;
        public int Length = 24;

        public Stack<Piece>[] Field
        {
            get => field;
            private set => field = value;
        }
        public List<Piece> Pieces
        {
            get => pieсes;
            private set => pieсes = value;
        }
        
        public GameBoard()
        {
            field = new Stack<Piece>[24];
            for (int i = 0; i < field.Length; ++i)
                field[i] = new Stack<Piece>();
            Pieces = new List<Piece>();
            for (int i = 0; i < 15; ++i)
            {
                Piece whitePiece = new Piece(Colors.White());
                Piece blackPiece = new Piece(Colors.Black());
                Pieces.Add(whitePiece);
                Pieces.Add(blackPiece);
                field[0].Push(whitePiece);
                field[23].Push(blackPiece);
                
            }
        }
       
        public Stack<Piece>[] GetField() => field;

        public Stack<Piece> GetCell(int i) => field[i];

       
    }
}
