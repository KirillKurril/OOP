namespace Entities.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public List<int> Storage { get; set; }
        public int GameBoardId {  get; set; }
        public GameBoard GameBoard { get; set; } = null!;
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
