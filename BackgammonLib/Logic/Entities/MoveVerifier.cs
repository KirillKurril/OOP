using BackgammonEntities;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Entities
{
    public class MoveVerifier
    {
        public int MyColor {  get; private set; }
        private int[] status ;
        private List<int> diceValues;
        private List<int> moveValues;
        private bool hatsOffToYou;
        private int firstChosenPosition;

        public MoveVerifier(int color)
        {
            MyColor = color;
        }

        public void Update(List<int> diceValues, List<int> moveValues,
            int[] status, bool hatsOffToYou)
        {
            this.diceValues = diceValues;
            this.moveValues = moveValues;
            this.hatsOffToYou = hatsOffToYou;
            this.status = status;
        }

        public bool VerifyStartPosition(int startPosition)
        {
            bool potentialMovesExist = MovsAvalibleExist();
            bool rigthColor = status[startPosition] == MyColor;
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
            for (int i = 0; i < 24; ++i)
                if (status[i] == MyColor)
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
            bool capturedByFriendlyUnit = status[destinatioin] == MyColor;

            return destExist && moveForvard && (isFree || capturedByFriendlyUnit);
        }
    }
}
