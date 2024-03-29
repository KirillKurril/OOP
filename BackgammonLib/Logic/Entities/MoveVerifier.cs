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
        public int Color {  get; set; }
        private int[] status ;
        private List<int> diceValues;
        private List<int> moveValues;
        private bool hatsOffToYou;

        public MoveVerifier(int color)
        {
            Color = color;
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
            bool rigthColor = status[startPosition] == Color;
            bool headless = !(hatsOffToYou && startPosition == 0);

            return potentialMovesExist && rigthColor && headless;
        }
        private bool MovsAvalibleExist()
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
                if (status[i] == Color)
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
            bool capturedByFriendlyUnit = status[destinatioin] == Color;

            return destExist && moveForvard && (isFree || capturedByFriendlyUnit);
        }
    }
}
