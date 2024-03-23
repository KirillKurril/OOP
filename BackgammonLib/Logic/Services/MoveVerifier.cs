using BackgammonEntities;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLogic.Services
{
    public class MoveVerifier
    {
        private int[] status;
        private (int, int) DetailedStatus;
        private List<int> diceValues;
        private List<int> moveValues;
        private bool hatsOffToYou;
        private Player curPlayer;
        private List<Cell> curField;

        public MoveVerifier(List<int> diceValues, List<int> moveValues,
            List<Cell> curField, Player curPlayer,
            bool hatsOffToYou)
        {
            this.diceValues = diceValues;
            this.moveValues = moveValues;
            this.curField = curField;
            this.curPlayer = curPlayer;
            this.hatsOffToYou = hatsOffToYou;
            
            status = new int[24];
            for (int i = 0; i < curField.Count; i++)
                status[i] = curField[i].GetColor();
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
            if (destinatioin > 23)
                return true;
            bool destExist = diceValues.Contains(destinatioin - source);          //must be move values
            bool moveForvard = source < destinatioin;
            bool isFree = status[destinatioin] == 0;
            bool capturedByFriendlyUnit = status[destinatioin] == curPlayer.Color;

            return destExist && moveForvard && (isFree || capturedByFriendlyUnit);
        }
    }
}
