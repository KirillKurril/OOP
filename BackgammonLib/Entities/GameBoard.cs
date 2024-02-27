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
            Pieces = new List<Piece>();
            for (int i = 0; i < 16; ++i)
            {
                field[i] = new Stack<Piece>();
                Piece whitePiece = new Piece(0);
                Piece blackPiece = new Piece(1);
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
