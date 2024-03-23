using BackgammonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class GameStatusData
    {
        public List<int> diceValues;
        public List<int> moveValues;
        public List<Cell> curField;
        public Player curPlayer;
        public bool hatsOffToYou;
        public List<Cell> whiteField;
        public int movesCounter;
        public bool NewTurn;
        public bool Victory;

        public GameStatusData(List<int> diceValues, List<int> moveValues, 
            List<Cell> curField, Player curPlayer, 
            bool hatsOffToYou, List<Cell> whiteField, 
            int movesCounter, bool newTurn, bool victory)
        {
            this.diceValues = diceValues;
            this.moveValues = moveValues;
            this.curField = curField;
            this.curPlayer = curPlayer;
            this.hatsOffToYou = hatsOffToYou;

            this.whiteField = whiteField;
            this.movesCounter = movesCounter;
            NewTurn = newTurn;
            Victory = victory;
        }
    }
}
