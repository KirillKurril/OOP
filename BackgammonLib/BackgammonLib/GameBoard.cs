using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    public class GameBoard
    {

        public Stack<Piece> [] Board;
        public List<Piece> Pieces;
        public List<int> moveValues;

        private Random randomizer;

        public void ChangePiecePosition(int firstcell, int secondCell) 
        {
            
        }

        public void RollTheDice()
        {
            int firstValue = randomizer.Next(1, 6);
            int secondValue = randomizer.Next(1, 6);
            moveValues.Add(firstValue);
            moveValues.Add(secondValue);
            if (firstValue == secondValue)
            {
                moveValues.Add(firstValue);
                moveValues.Add(firstValue);
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
