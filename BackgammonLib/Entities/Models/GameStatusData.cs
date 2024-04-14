using System.Text;

namespace Entities.Models
{
    public class GameStatusData
    {
        public List<int> Status { get; set; }
        public List<int> DiceValues { get; set; }
        public List<int> MoveValues { get; set; }
        public bool ReachedHome { get; set; }
        public bool HatsOffToYou { get; set; }
        public List<(int, int)> ExtraStatus { get; set; }
        public int MoveColor { get; set; }
        public (int, int) Score { get; set; }
        public (bool, bool) Safemode { get; set; }
        public (bool, int) EndGame { get; set; }

        public GameStatusData(
            List<int> status, List<int> diceValues, List<int> moveValues, bool reachedHome, bool hatsOffToYou,
            List<(int, int)> extraStatus,
            int moveColor, (int, int) score, (bool, bool) safemode, (bool, int) endGame)
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
            EndGame = endGame;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string statusStr = string.Empty;
            foreach (var stat in Status)
                statusStr += $"[{stat}] ";
            sb.Append($"Status: {statusStr}\n");

            string dices = "Dice values: \n";
            foreach (var diceValue in DiceValues)
                dices += $"{diceValue}\n";
            sb.Append(dices);

            string moves = "Move values: \n";
            foreach (var moveValue in DiceValues)
                dices += $"{moveValue}\n";
            sb.Append(moves);

            sb.Append($"White's in safe: {Safemode.Item1}\nBlack's in safe: {Safemode.Item2}\n");

            sb.Append($"You Reached home: {ReachedHome}\n");

            sb.Append($"You took your hat off: {HatsOffToYou} \n");

            string extraStatus = "Extra status:";
            int counter = 0;
            foreach (var pair in ExtraStatus)
            {
                extraStatus += $"\n[{counter++}] ";
                for (int i = 0; i < pair.Item2; i++)
                    extraStatus += pair.Item1 == Colors.WhitePiece ? 'w' : 'b';//'○' : '●';
            }

            sb.Append(extraStatus + '\n');

            sb.Append($"MoveColor: {MoveColor}\n");

            sb.Append($"Score: {Score}\n");

            sb.Append($"It's the end of the game: {EndGame.Item1}\n" +
                "If so, then the winner is " + (EndGame.Item2 == Colors.WhitePiece ? "White" : "Black") + '\n');


            return sb.ToString();
        }
    }
}
