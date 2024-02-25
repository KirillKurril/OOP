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
        private Player whitePlayer;
        private Player BlackPlayer;
        private GameBoard gameBoard;

        public (bool, int) CheckEndTurn()
        {
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
    }
}
