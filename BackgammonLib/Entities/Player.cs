using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonEntities
{
    public class Player
    {
        private int color;
        public int Score;

        public bool ReachedHome;
        public bool SafeMode;

        public int Color
        {
            get => color;
            private set => color = value;
        }


        public Player(int color_)
            => (Color, Score, SafeMode) = (color_, 360, true);

    }
}
