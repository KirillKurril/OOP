using BackgammonEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Cell
    {
        private Stack<Piece> storage;
        public Cell() => storage = new Stack<Piece>();

        public void Push(Piece piece) => storage.Push(piece);

        public Piece Pop() => storage.Pop();

        public Piece CheckUpper()
            => storage.Peek();

        public bool IsEmpty() => storage.Count == 0;

        public int GetColor()
        {
            if(IsEmpty())
                return 0;
            if (storage.Peek().Color == Colors.White())
                return Colors.White();
            else
                return Colors.Black();

        }

        public int GetHeight()
            => storage.Count;
    }
}
