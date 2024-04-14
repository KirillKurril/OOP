//Логичнее здесь было бы реализовывать в виде основной структуры данных стек, но он не является элементарным типом данных и его нельзя записать в бд
using System.ComponentModel.DataAnnotations.Schema;
using Entities.GameServices;

namespace Entities.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public List<int> Storage { get; set; }

        [ForeignKey("WhiteField")]
        public int WhiteFieldId { get; set; }
        public GameBoard? WhiteField { get; set; }

        [ForeignKey("BlackField")]
        public int BlackFieldId { get; set; }
        public GameBoard? BlackField { get; set; }

        public int NetGameId { get; set; }
        public NetGame? NetGame { get; set; }

        public Cell()
            => Storage = new List<int>();
        public void Push(int piece)
            => Storage.Append(piece);
        public int Pop()
        {
            int lastPiece = Storage[Storage.Count - 1];
            Storage.RemoveAt(Storage.Count - 1);
            return lastPiece;
        }
        public int CheckUpper()
            => Storage[Storage.Count - 1];
        public bool IsEmpty()
            => Storage.Count == 0;
        public int GetColor()
        {
            if (IsEmpty())
                return 0;
            if (Storage[Storage.Count - 1] == Colors.WhitePiece)
                return Colors.White;
            else
                return Colors.Black;
        }
        public int GetHeight()
            => Storage.Count;
    }
}
