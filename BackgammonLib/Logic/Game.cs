using System;
using System.Collections.Generic;
using System.Drawing;
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
        public Player curPlayer;
        private GameBoard board;
        private Random randomizer;
        private int[] status;
        public List<int> diceValues; //значения на костях 
        public List<int> moveValues; //доступные ходы
        


        public Game()
        {
            curPlayer = new Player(0);
            whiteScore = 360;
            blackScore = 360;
            board = new GameBoard();
            randomizer = new Random();
            moveValues = new List<int>();
            diceValues = new List<int>();
            int[] status = new int[board.Length];
        }
        public void RollDices()
        {
            int firstValue = randomizer.Next(1, 6);
            int secondValue = randomizer.Next(1, 6);
            diceValues.Add(firstValue);
            diceValues.Add(secondValue);
            if (firstValue == secondValue)
            {
                diceValues.Add(firstValue);
                diceValues.Add(firstValue);
            }
        }

        private void RefreshMoveValues()
        {
            moveValues.Clear();
            if(diceValues.Count == 4)
            {
                moveValues.Add(diceValues[1]);
                moveValues.Add(diceValues[1] * 2);
                moveValues.Add(diceValues[1] * 3);
                moveValues.Add(diceValues[1] * 4);
            }
            if (diceValues.Count == 3)
            {
                moveValues.Add(diceValues[1]);
                moveValues.Add(diceValues[1] * 2);
                moveValues.Add(diceValues[1] * 3);
            }
            if (diceValues.Count == 2)
            {
                moveValues.Add(diceValues[0]);
                moveValues.Add(diceValues[1]);
                moveValues.Add(diceValues[0] + diceValues[1]);
            }
            if (diceValues.Count == 1)
                moveValues.Add(diceValues[0]);
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
            diceValues.Remove(Math.Abs(source - destination));
            RefreshMoveValues();

        }
        private void ThrowOut(int position)
        {
            var throwedPiece = board.field[position].Pop();
            board.Pieces.Remove(throwedPiece);
        }
        public bool ReachedHome()
        {
            var situation = Status();
            if(curPlayer.Color == 0)
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

        public int[] GetStatus() => status;
        private void RefreshStatus()
        {
            for (int i = 0; i < board.Length; i++)
            {
                var cell = board.GetCell(i);
                if (cell.TryPeek(out Piece? piece))
                    status[i] = piece.Color;
                else
                    status[i] = -1;
            }
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

        public bool MovsAvalibleExist()
        {
            return true;
        }

        public bool GetLegalPositions(int position, out List<int> avaliblePositions)
        {
            avaliblePositions = moveValues.Where().ToList();
            if (curPlayer.ReachedHome)
                avaliblePositions.Add(-1);
            
            return true;
        }

        public void NewTurn()
        {
            curPlayer.Color = curPlayer.Color == 0 ? 1 : 0;
            RollDices();
            RefreshMoveValues();

        }
    }
}
