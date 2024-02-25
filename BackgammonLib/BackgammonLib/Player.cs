using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    internal class Player
    {
        private int color;
        public int Color
        {
            get => color;
            private set => color = value;
        }

        public Player(int color_)
            => color = color_;

    }
}
