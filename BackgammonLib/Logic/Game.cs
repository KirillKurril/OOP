﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Player[] players;
        private List<Cell> curField;
        private GameBoard board;
        private Random randomizer;
        private int[] status;           //массив с цветами позиций по текущему полю
        private List<int> diceValues;   //значения на костях 
        private List<int> moveValues;   //доступные ходы

        private bool hatsOffToYou;
        private int movesCounter;

        public Game()
        {
            board = new GameBoard();

            players = new Player[2];
            players[0] = new Player(Colors.White());
            players[1] = new Player(Colors.Black());
            curPlayer = players[0];

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

            ReachedHomeRefresh();
        }


        public bool VerifyStartPosition(int startPosition)
        {
            bool potentialMovesExist = MovsAvalibleExist();
            bool rigthColor = status[startPosition] == curPlayer.Color;
            bool headless = !(hatsOffToYou && startPosition == 0);

            return potentialMovesExist && rigthColor && headless;
        }
        public bool MovsAvalibleExist()
        {
            if (diceValues.Count > 0)
            {
                List<int> monitoredPositions = GetMonitoredPositions();

                if (hatsOffToYou && monitoredPositions.Contains(0))
                    monitoredPositions.Remove(0);

                return monitoredPositions.Any(position
                    => diceValues.Any(shift => MoveConfirm(position, position + shift)));      //here must be move values
            }
            return false;
        }
        private List<int> GetMonitoredPositions()
        {
            List<int> positions = new List<int>();
            for (int i = 0; i < curField.Count; ++i)
                if (curField[i].GetColor() == curPlayer.Color)
                    positions.Add(i);
            return positions;
        }
        public bool MoveConfirm(int source, int destinatioin)
        {
            if (!curField[source].IsEmpty() && source < destinatioin)
            {
                if (GetLegalPositions(source, out List<int> avaliblePositions))
                    return avaliblePositions.Contains(destinatioin);
            }
            return false;
        }
        private bool GetLegalPositions(int position, out List<int> avaliblePositions)
        {
            avaliblePositions = new List<int>();

            List<int> potentialMoves = diceValues.Select(shiftPosition          //here must be move values
                => shiftPosition + position).ToList(); 

            foreach(var potentialDest in potentialMoves.Where(dest => dest < 24))
            {
                bool isFree = status[potentialDest] == 0;
                bool capturedByFriendlyUnit = status[potentialDest] == curPlayer.Color;
                if (isFree || capturedByFriendlyUnit)
                    avaliblePositions.Add(potentialDest);
            }

            bool throwAway = potentialMoves.Any(potentialDest => potentialDest > 23);

            if (throwAway)
                avaliblePositions.Add(25);

            return avaliblePositions.Count == 0 ? false : true;
        }
        public void Move(int source, int destination)
        {
            if (destination == 25)
            {
                destination = ThrowOut(source);
                Refresh(destination, source, true);
            }
            else
            {
                var movingPiece = curField[source].Pop();
                curField[destination].Push(movingPiece);

                if (!hatsOffToYou && source == 0)
                    hatsOffToYou = true;
                Refresh(destination, source);
            }

        }
        private int ThrowOut(int position)
        {
            var throwedPiece = curField[position].Pop();
            List<int> throwDices = diceValues.Where(diceValue => diceValue + position >= 24).ToList();
            int distance = throwDices.Min();
            return distance + position;
        }
        public void NewTurn()
        {
            hatsOffToYou = false;
            curPlayer = curPlayer.Color == 1 ? players[1] : players[0];
            curField = curField == board.BlackField ? board.WhiteField : board.BlackField;
            StatusRefresh();
            RollDices();
            MoveValuesRefresh();
        }
        
        
        public List<(int, int)> GetDetailedReport()
        {
            List<(int, int)> report = new List<(int, int)>();

            foreach (var cell in board.whiteField)
                report.Add((cell.GetColor(), cell.GetHeight()));

            return report;
        }
        public (int, int) GetScore()
           => (players[0].Score, players[1].Score);
        public bool GetPositionEctability(int position)
        {
            List<int> throwDices = diceValues.Where(diceValue => diceValue + position >= 24).ToList();
            return throwDices.Count != 0;
        }
        public int[] GetStatus() => status;
        public List<int> GetDiceValues() => diceValues;
        public int GetPlayerColor() => curPlayer.Color;
        public bool GetPlayerStatus() => curPlayer.ReachedHome;
        public int GetCurColor() => curPlayer.Color;


        void Refresh(int destination, int source, bool throwCase = false)
        {
            RemoveUsedDices(destination - source);
            ScoreRefresh(throwCase ? 24 - source : destination - source);
            MoveValuesRefresh();
            StatusRefresh();
            SafeModeRefresh();
            ReachedHomeRefresh();
        }
        private void SafeModeRefresh()
            => curPlayer.SafeMode = status.Skip(18).All(position => position != curPlayer.Color);
        private void StatusRefresh()
        {
            for (int i = 0; i < curField.Count; i++)
                status[i] = curField[i].GetColor();
        }
        private void ReachedHomeRefresh()
            => curPlayer.ReachedHome = status.Take(18).All(position => position != curPlayer.Color);
        private void ScoreRefresh(int modul)
            => curPlayer.Score -= modul;
        public bool CheckEndGame() => curPlayer.Score == 0;
        public void RollDices()
        {
            diceValues.Clear();
            int firstValue = randomizer.Next(1, 7);
            int secondValue = randomizer.Next(1, 7);
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

            if(diceValues.Count > 2)
                for(int i = diceValues.Count; i > 0; --i)
                    moveValues.Add(diceValues[0] * i);
            else if (diceValues.Count == 2)
            {
                moveValues = new List<int>(diceValues);
                moveValues.Add(diceValues.Sum());
            }
            else
                moveValues = new List<int>(diceValues);
        }
        private void RemoveUsedDices(int moveModul)
        {
            if (diceValues.Count >= 3)
                for (int i = moveModul / diceValues[0]; i > 0; --i)
                    diceValues.Remove(diceValues[0]);

            else if (moveModul == diceValues.Sum())
                diceValues.Clear();
            else
                diceValues.Remove(moveModul);
        }

    }
}
