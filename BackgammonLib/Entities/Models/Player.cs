using Entities.GameServices;

namespace Entities.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int Color { get; set; }
        public int Score { get; set; }
        public bool ReachedHome { get; set; }
        public bool SafeMode { get; set; }
        public int NetGameId { get; set; }
        public NetGame NetGame { get; set; } = null!;

        public Player(int color, int score = 360)
            => (Color, Score, SafeMode) = (color, score, true);
    }
}
