using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonEntities
{
    public class Player
    {
        public int Color;
        public int Score;

        public bool ReachedHome;

        public Player(int color_)
            => (Color, Score) = (color_, 360);

    }
}
