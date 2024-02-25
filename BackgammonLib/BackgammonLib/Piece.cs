﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonLib
{
    public class Piece
    {
        private string color;
        private int position;

        public string Color
        {
            get => color;
            private set => color = value; 
        }

        public int Position
        {
            get => position;
            private set => position = value;
        }

        public bool Ejactable
        {
            get => position > 18;
            private set { }
        }

        public Piece(string color_)
            => (color, position) = (color_, 1);

        public void MoveTo(int newPosition)
            => position = newPosition;

    }
}
