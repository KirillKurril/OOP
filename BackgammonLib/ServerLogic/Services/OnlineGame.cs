﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic.Interfaces;
using Entities;
using BackgammonEntities;

namespace ServerLogic.Services
{
    public class OnlineGame : IGame
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

        public OnlineGame()
        {
            board = new GameBoard();

            players = new Player[2];
            players[0] = new Player(Colors.White());
            players[1] = new Player(Colors.Black());
            curPlayer = players[0];

            curField = board.WhiteField;

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
            curField[position].Pop();
            List<int> throwDices = diceValues.Where(diceValue => diceValue + position >= 24).ToList();
            int distance = throwDices.Min();
            return distance + position;
        }
        public void NewTurn()
        {
            //do
            //{
                hatsOffToYou = false;
                curPlayer = curPlayer.Color == 1 ? players[1] : players[0];
                curField = curField == board.BlackField ? board.WhiteField : board.BlackField;
                StatusRefresh();
                RollDices();
                MoveValuesRefresh();
            //} while (!MovsAvalibleExist());

        }
        public List<(int, int)> GetDetailedReport()
        {
            List<(int, int)> report = new List<(int, int)>();

            foreach (var cell in board.WhiteField)
                report.Add((cell.GetColor(), cell.GetHeight()));

            return report;
        }
        public (int, int) GetScore()
           => (players[0].Score, players[1].Score);
        public bool GetPositionEctability(int position)
        {
            List<int> throwDices = diceValues.Where(diceValue => diceValue + position >= 24).ToList();
            return throwDices.Count != 0 && curPlayer.ReachedHome;
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

            if (diceValues.Count > 2)
                for (int i = diceValues.Count; i > 0; --i)
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
