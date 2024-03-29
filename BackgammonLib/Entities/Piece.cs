﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonEntities
{
    public class Piece
    {
        private int color;
        private int position;
        public int Color
        {
            get => color;
            private set => color = value; 
        }

        public int Position
        {
            get => position;
            private set => position = value;
        }

        public Piece(int color_)
            => (color, position) = (color_, 0);
    }
}
