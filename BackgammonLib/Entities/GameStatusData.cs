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
        public int[] Status {  get; set; }
        public List<int> DiceValues { get; set; }
        public List<int> MoveValues { get; set; }
        public bool ReachedHome { get; set; }
        public bool HatsOffToYou { get; set; }
        public List<(int, int)> ExtraStatus { get; set; }
        public int MoveColor { get; set; }
        public (int, int) Score { get; set; }
        public bool Throwable { get; set; }
        public bool Safemode { get; set; }

        public GameStatusData(
            int[] status,List<int> diceValues, List<int> moveValues, bool reachedHome, bool hatsOffToYou,
            List<(int, int)> extraStatus,
            int moveColor, (int, int) score, bool throwable, bool safemode)
        {
            Status = status;
            DiceValues = diceValues;
            MoveValues = moveValues;
            ReachedHome = reachedHome;
            HatsOffToYou = hatsOffToYou;
            Safemode = safemode;
            ExtraStatus = extraStatus;
            MoveColor = moveColor;
            Score = score;
            Throwable = throwable;
        }

        
    }
}
