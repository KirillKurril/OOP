using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    public class Game
    {
        private int whiteScore;
        private int blackScore;
        public Player whitePlayer;
        public Player blackPlayer;
        private GameBoard gameBoard;
        private Random randomizer;
        public List<int> moveValues;


        public Game()
        {
            whitePlayer = new Player(0);
            blackPlayer = new Player(1);
            whiteScore = 360;
            blackScore = 360;
            gameBoard = new GameBoard();
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
            foreach (var piece in gameBoard.Pieces)
            {
                if (piece.Color == 1)
                    whiteScore += 24 - piece.Position;
                else
                    blackScore += piece.Position;
            }
        }

        public int[] GetSituation() => gameBoard.Status();

        public void InitMove(int pos1, int pos2) => gameBoard.Move(pos1, pos2);

        public bool ReachedHome(Player player)
        {
            var situation = gameBoard.Status();
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

        public Stack<Piece>[] GetDetailedReport() => gameBoard.GetBoard();
    }
}
