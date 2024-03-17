using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BackgammonEntities;
using Entities;

//белый был 0, стал 1, чернный был 1, стал -1

namespace BackgammonLogic
{
    public class Game
    {
        private Player curPlayer;
        private Player[] Players;
        private GameBoard board;
        private Random randomizer;
        private int[] status;        //массив с цветами позиций
        private List<int> diceValues; //значения на костях 
        private List<int> moveValues; //доступные ходы
        
        public int GetPlayerColor() => curPlayer.Color;
        public bool GetPlayerStatus() => curPlayer.ReachedHome;

        public Game()
        {
            board = new GameBoard();

            Players = new Player[2];
            Players[0] = new Player(Colors.White());
            Players[1] = new Player(Colors.Black());
            curPlayer = Players[1];

            randomizer = new Random();
            moveValues = new List<int>();
            diceValues = new List<int>();

            status = new int[24];
            StatusRefresh();
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
        private void MoveValuesRefresh()
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
        public bool CheckEndGame() => curPlayer.Score == 0;
        public void Move(int source, int destination)
        {
            if (destination == -1)
                ThrowOut(source);
            else
            {
                var movingPiece = board.field[source].Pop();
                board.field[destination].Push(movingPiece);
            }

            diceValues.Remove(Math.Abs(source - destination));
            MoveValuesRefresh();
            StatusRefresh();
            ReachedHomeRefresh();
        }
        private void ThrowOut(int position)
        {
            var throwedPiece = board.field[position].Pop();
            board.Pieces.Remove(throwedPiece);
        }
        public bool ReachedHomeRefresh()
        {
            if(curPlayer.Color == Colors.Black())
            {
                for(int i = 12; i < 18; i++)
                    if (status[i] == Colors.Black())
                        return true;
                return false;
            }
            else
            {
                for (int i = 6; i < 12; i++)
                    if (status[i] == Colors.White())
                        return true;
                return false;
            }
        }
        public int[] GetStatus() => status;
        private void StatusRefresh()
        {
            for (int i = 0; i < board.Length; i++)
            {
                var cell = board.GetCell(i);
                if (cell.TryPeek(out Piece? piece))
                    status[i] = piece.Color;
                else
                    status[i] = 0;
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
                    report[i].Item1 = 0;
                    report[i].Item2 = 0;
                }
            }
            return report;
        }
        private List<Piece> GetMonitoredPieces() 
            => board.Pieces.Where(piece => piece.Color == curPlayer.Color).ToList();
        public bool MovsAvalibleExist()// => GetMonitoredPieces().All(piece => GetLegalPositions(piece, out List<int> avaliblePositions) == true);
        {
            List<Piece> pieces = GetMonitoredPieces();
            return pieces.All(piece => GetLegalPositions(piece, out List<int> avaliblePositions) == true);

        }
        public bool GetLegalPositions(Piece piece, out List<int> avaliblePositions)
        {
            avaliblePositions = new List<int>();

            List<int> potentialMoves = moveValues.Select(shiftPosition
                => shiftPosition * curPlayer.Color + piece.Position).ToList(); 

            foreach(var position in potentialMoves)
            {
                bool isFree = status[position] == 0;
                bool capturedByFriendlyUnit = status[position] == curPlayer.Color;
                if (isFree || capturedByFriendlyUnit)
                    avaliblePositions.Add(position);
            }

            bool throwAway = curPlayer.ReachedHome && 
                piece.Color == Colors.White()? 
                piece.Position < 18 && piece.Position >= 12 
                : piece.Position < 12 && piece.Position >= 6;

            if (throwAway)
                avaliblePositions.Add(-1);

            return avaliblePositions.Count == 0 ? false : true;
        }
        public bool MoveConfirm(int source, int destinatioin)
        {
            var cell = board.GetCell(source);
            if (cell.TryPeek(out Piece? piece))
            {
                if(GetLegalPositions(piece, out List<int> avaliblePositions))
                    return avaliblePositions.Contains(destinatioin);
            }
            return false;
        }
        public List<int> NewTurn()
        {
            curPlayer = curPlayer.Color == 1 ? Players[1] : Players[0];
            RollDices();
            MoveValuesRefresh();
            return diceValues;
        }
    }
}
