using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class GameBoard
    {

        public Stack<Piece> [] Board;
        public List<Piece> Pieces;

        public GameBoard()
        {
            Board = new Stack<Piece>[24];
            Pieces = new List<Piece>();
            for (int i = 0; i < 16; ++i)
            {
                Piece whitePiece = new Piece(0);
                Piece blackPiece = new Piece(1);
                Pieces.Add(whitePiece);
                Pieces.Add(blackPiece);
                Board[0].Push(whitePiece);
                Board[23].Push(blackPiece);
            }
        }
       

        public int[] Status()
        {
            int[] report = new int[Board.Length];
             for(int i = 0; i < Board.Length; i++)
            {
                if (Board[i].TryPeek(out Piece? piece))
                    report[i] = piece.Color;
                else
                    report[i] = -1;
            }
            return report;
        }

        public void Move(int source, int destination)
        {
            if (destination == 24)
                ThrowOut(source);
            else
            {
                var movingPiece = Board[source].Pop();
                Board[destination].Push(movingPiece);
            }
        }

        private void ThrowOut(int position)
        {
            var throwedPiece = Board[position].Pop();
            Pieces.Remove(throwedPiece);
        }
    }
}
