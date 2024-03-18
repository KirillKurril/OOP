using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BackgammonEntities;
using Entities;


namespace BackgammonLogic
{
    public class Game
    {
        private Player curPlayer;
        private Player[] Players;
        private List<Cell> curField;
        private GameBoard board;
        private Random randomizer;
        private int[] status;           //массив с цветами позиций по текущему полю
        private List<int> diceValues;   //значения на костях 
        private List<int> moveValues;   //доступные ходы

        private bool hatsOffToYou;
        private int movesCounter;

        public int GetPlayerColor() => curPlayer.Color;
        public bool GetPlayerStatus() => curPlayer.ReachedHome;
        public Game()
        {
            board = new GameBoard();

            Players = new Player[2];
            Players[0] = new Player(Colors.White());
            Players[1] = new Player(Colors.Black());
            curPlayer = Players[0];

            curField = board.whiteField;

            randomizer = new Random();
            moveValues = new List<int>();
            diceValues = new List<int>();

            status = new int[24];
            StatusRefresh();

            hatsOffToYou = false;
            movesCounter = 1;

            RollDices();
            MoveValuesRefresh();
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
                var movingPiece = curField[source].Pop();
                curField[destination].Push(movingPiece);
            }

            if(!hatsOffToYou)
                hatsOffToYou = true;

            diceValues.Remove(Math.Abs(source - destination));
            MoveValuesRefresh();
            StatusRefresh();
        }
        private void ThrowOut(int position)
        {
            var throwedPiece = curField[position].Pop();
            board.Pieces.Remove(throwedPiece);
        }
        public void ReachedHomeRefresh()
            => curPlayer.ReachedHome = status.All(position => position > 17);
        public int[] GetStatus() => status;
        private void StatusRefresh()
        {
            for (int i = 0; i < curField.Count; i++)
                status[i] = curField[i].GetColor();
        }

        public List<(int, int)> GetDetailedReport()
        {
            List<(int, int)> report = new List<(int, int)>();

            foreach (var cell in board.whiteField)
                report.Add((cell.GetColor(), cell.GetHeight()));
            
            return report;
        }

        public List<int> GetMonitoredPositions()
        {
            List<int> positions = new List<int>();
            for (int i = 0; i < curField.Count; ++i)
                if (curField[i].GetColor() == curPlayer.Color)
                    positions.Add(i);
            return positions;
        }
        public bool MovsAvalibleExist()
        {
            if(moveValues.Count > 0)
            {
                return GetMonitoredPositions().All(position
                    => GetLegalPositions(position, out List<int> avaliblePositions) == true);
            }
            return false;
        }
        public bool GetLegalPositions(int position, out List<int> avaliblePositions)
        {
            avaliblePositions = new List<int>();

            List<int> potentialMoves = moveValues.Select(shiftPosition
                => shiftPosition + position).ToList(); 

            foreach(var potentialDest in potentialMoves)
            {
                bool isFree = status[potentialDest] == 0;
                bool capturedByFriendlyUnit = status[potentialDest] == curPlayer.Color;
                if (isFree || capturedByFriendlyUnit)
                    avaliblePositions.Add(potentialDest);
            }

            bool throwAway = potentialMoves.Any(potentialDest => potentialDest > 23);

            if (throwAway)
                avaliblePositions.Add(-1);

            return avaliblePositions.Count == 0 ? false : true;
        }
        public bool MoveConfirm(int source, int destinatioin)
        {

            //////////////////////////
            Debug.WriteLine($"Bashka na meste: {hatsOffToYou}");
            Debug.WriteLine($"Checked move: {source}, {destinatioin}");
            //////////////////////////
            
            if (!curField[source].IsEmpty())
            {
                if(GetLegalPositions(source, out List<int> avaliblePositions))
                    return avaliblePositions.Contains(destinatioin);
            }
            return false;

            
        }
        public void NewTurn()
        {
            hatsOffToYou = false;
            curPlayer = curPlayer.Color == 1 ? Players[1] : Players[0];
            curField = curField == board.BlackField ? board.WhiteField : board.BlackField;
            StatusRefresh();
            RollDices();
            MoveValuesRefresh();

            //////////////////////////
            Debug.WriteLine(movesCounter++);
            //////////////////////////
        }
        public List<int> GetDiceValues() => diceValues;

        public bool VerifyStartPosition(int startPosition)
        {
            bool potentialMovesExist = MovsAvalibleExist();
            bool rigthColor = status[startPosition] == curPlayer.Color;
            bool headless = !(hatsOffToYou && startPosition == 0);

            return potentialMovesExist && rigthColor && headless;
        }
    }
}
