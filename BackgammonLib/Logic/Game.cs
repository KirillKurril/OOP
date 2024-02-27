using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackgammonEntities;

namespace BackgammonLogic
{
    public class Game
    {
        private int whiteScore;
        private int blackScore;
        public Player whitePlayer;
        public Player blackPlayer;
        private GameBoard board;
        private Random randomizer;
        public List<int> moveValues;


        public Game()
        {
            whitePlayer = new Player(0);
            blackPlayer = new Player(1);
            whiteScore = 360;
            blackScore = 360;
            board = new GameBoard();
            randomizer = new Random();
            moveValues = new List<int>();   
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
        public (bool, int) CheckEndTurn()
        {
            // bool next turn pending int who won
            if (whiteScore == 0 || blackScore == 0)
                return (false, whiteScore == 0 ? 0 : 1);
            else
                return (true, -1);
        }
        public void GetScore()
        { 
            blackScore = 0;
            whiteScore = 0;
            foreach (var piece in board.Pieces)
            {
                if (piece.Color == 1)
                    whiteScore += 24 - piece.Position;
                else
                    blackScore += piece.Position;
            }
        }
        public void Move(int source, int destination)
        {
            if (destination == 24)
                ThrowOut(source);
            else
            {
                var movingPiece = board.field[source].Pop();
                board.field[destination].Push(movingPiece);
            }
        }
        private void ThrowOut(int position)
        {
            var throwedPiece = board.field[position].Pop();
            board.Pieces.Remove(throwedPiece);
        }
        public bool ReachedHome(Player player)
        {
            var situation = Status();
            if(player.Color == 0)
            {
                for(int i = 12; i < 18; i++)
                    if (situation[i] == 0)
                        return true;
                return false;
            }
            else
            {
                for (int i = 6; i < 12; i++)
                    if (situation[i] == 1)
                        return true;
                return false;
            }
        }

        public int[] Status()
        {
            int[] report = new int[board.Length];
            for (int i = 0; i < board.Length; i++)
            {
                var cell = board.GetCell(i);
                if (cell.TryPeek(out Piece? piece))
                    report[i] = piece.Color;
                else
                    report[i] = -1;
            }
            return report;
        }

        public (int, int)[] GetDetailedReport()
        {
            (int, int)[] report = new (int, int)[board.Length];

            for (int i = 0; i < board.Length; i++)
            {
                var cell = board.GetCell(i);
                if (cell.TryPeek(out Piece? piece))
                {
                    report[i].Item1 = piece.Color;
                    report[i].Item2 = cell.Count;
                }
                else
                {
                    report[i].Item1 = -1;
                    report[i].Item2 = 0;
                }
                    
                    
            }
            return report;
        }

    }
}
