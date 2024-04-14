using Entities.Models;
using ServerDB.Models;


namespace Entities.GameServices
{
    public class NetGame
    {
        public int Id { get; set; }
        public Player CurPlayer { get; set; }
        public List<Player> Players { get; set; }
        public List<Cell> СurField { get; set; }
        public GameBoard Board { get; set; }
        public List<int> Status { get; set; }           
        public List<int> DiceValues { get; set; }   
        public List<int> MoveValues { get; set; }   
        public bool HatsOffToYou { get; set; }
        public int RoomId {  get; set; }
        public Room? Room { get; set; }
        
        public NetGame()
        {
            Board = new GameBoard();

            Players = new List<Player>(2);
            Players[0] = new Player(Colors.White);
            Players[1] = new Player(Colors.Black);
            CurPlayer = Players[0];

            СurField = Board.WhiteField;

            MoveValues = new List<int>();
            DiceValues = new List<int>();

            Status = new List<int>(24);
            StatusRefresh();

            HatsOffToYou = false;

            RollDices();
            MoveValuesRefresh();

            ReachedHomeRefresh();
        }
        public bool VerifyStartPosition(int startPosition)
        {
            bool potentialMovesExist = MovsAvalibleExist();
            bool rigthColor = Status[startPosition] == CurPlayer.Color;
            bool headless = !(HatsOffToYou && startPosition == 0);

            return potentialMovesExist && rigthColor && headless;
        }
        public bool MovsAvalibleExist()
        {
            if (DiceValues.Count > 0)
            {
                List<int> monitoredPositions = GetMonitoredPositions();

                if (HatsOffToYou && monitoredPositions.Contains(0))
                    monitoredPositions.Remove(0);

                return monitoredPositions.Any(position
                    => DiceValues.Any(shift => MoveConfirm(position, position + shift)));      //here must be move values
            }
            return false;
        }
        private List<int> GetMonitoredPositions()
        {
            List<int> positions = new List<int>();
            for (int i = 0; i < СurField.Count; ++i)
                if (СurField[i].GetColor() == CurPlayer.Color)
                    positions.Add(i);
            return positions;
        }
        public bool MoveConfirm(int source, int destinatioin)
        {
            if (destinatioin > 23)
                return true;
            bool destExist = DiceValues.Contains(destinatioin - source);          //must be move values
            bool moveForvard = source < destinatioin;
            bool isFree = Status[destinatioin] == 0;
            bool capturedByFriendlyUnit = Status[destinatioin] == CurPlayer.Color;
            
            return (destExist && moveForvard && (isFree || capturedByFriendlyUnit));
        }
        public void Move(int source, int destination)
        {
            if (destination == 25)
            {
                destination = ThrowOut(source);
                Refresh(destination, source, true);
            }
            else
            {
                var movingPiece = СurField[source].Pop();
                СurField[destination].Push(movingPiece);

                if (!HatsOffToYou && source == 0)
                    HatsOffToYou = true;
                Refresh(destination, source);
            }

        }
        private int ThrowOut(int position)
        {
            СurField[position].Pop();
            List<int> throwDices = DiceValues.Where(diceValue => diceValue + position >= 24).ToList();
            int distance = throwDices.Min();
            return distance + position;
        }
        public void NewTurn()
        {
            do
            {
                HatsOffToYou = false;
                CurPlayer = CurPlayer.Color == 1 ? Players[1] : Players[0];
                СurField = СurField == Board.BlackField ? Board.WhiteField : Board.BlackField;
                StatusRefresh();
                RollDices();
                MoveValuesRefresh();
            } while (!MovsAvalibleExist());
           
        }
        public List<(int, int)> GetDetailedReport()
        {
            List<(int, int)> report = new List<(int, int)>();

            foreach (var cell in Board.WhiteField)
                report.Add((cell.GetColor(), cell.GetHeight()));

            return report;
        }
        public (int, int) GetScore()
           => (Players[0].Score, Players[1].Score);
        public bool GetPositionEctability(int position)
        {
            List<int> throwDices = DiceValues.Where(diceValue => diceValue + position >= 24).ToList();
            return throwDices.Count != 0 && CurPlayer.ReachedHome;
        }
        public List<int> GetStatus()
            => Status;
        public GameStatusData GetGameStatus()
        {
            GameStatusData data = new GameStatusData(
                Status,
                DiceValues,
                MoveValues,
                CurPlayer.ReachedHome,
                HatsOffToYou,
                GetDetailedReport(),
                GetCurColor(),
                (Players[0].Score, Players[1].Score),
                (Players[0].SafeMode, Players[1].SafeMode),
                (CheckEndGame(), GetCurColor())
                );
            return data;
        }
        public List<int> GetDiceValues()
            => DiceValues;
        public int GetPlayerColor()
            => CurPlayer.Color;
        public bool GetPlayerStatus()
            => CurPlayer.ReachedHome;
        public int GetCurColor()
            => CurPlayer.Color;
        void Refresh(int destination, int source, bool throwCase = false)
        {
            RemoveUsedDices(destination - source);
            ScoreRefresh(throwCase ? 24 - source : destination - source);
            MoveValuesRefresh();
            StatusRefresh();
            SafeModeRefresh();
            ReachedHomeRefresh();
        }
        private void SafeModeRefresh()
            => CurPlayer.SafeMode = Status.Skip(18).All(position => position != CurPlayer.Color);
        private void StatusRefresh()
        {
            for (int i = 0; i < СurField.Count; i++)
                Status[i] = СurField[i].GetColor();
        }
        private void ReachedHomeRefresh()
            => CurPlayer.ReachedHome = Status.Take(18).All(position => position != CurPlayer.Color);
        private void ScoreRefresh(int modul)
            => CurPlayer.Score -= modul;
        public bool CheckEndGame()
            => CurPlayer.Score == 0;
        public void RollDices()
        {
            DiceValues.Clear();
            int firstValue = new Random().Next(1, 7);
            int secondValue = new Random().Next(1, 7);
            DiceValues.Add(firstValue);
            DiceValues.Add(secondValue);
            if (firstValue == secondValue)
            {
                DiceValues.Add(firstValue);
                DiceValues.Add(firstValue);
            }
        }
        private void MoveValuesRefresh()
        {
            MoveValues.Clear();

            if(DiceValues.Count > 2)
                for(int i = DiceValues.Count; i > 0; --i)
                    MoveValues.Add(DiceValues[0] * i);
            else if (DiceValues.Count == 2)
            {
                MoveValues = new List<int>(DiceValues);
                MoveValues.Add(DiceValues.Sum());
            }
            else
                MoveValues = new List<int>(DiceValues);
        }
        private void RemoveUsedDices(int moveModul)
        {
            if (DiceValues.Count >= 3)
                for (int i = moveModul / DiceValues[0]; i > 0; --i)
                    DiceValues.Remove(DiceValues[0]);

            else if (moveModul == DiceValues.Sum())
                DiceValues.Clear();
            else
                DiceValues.Remove(moveModul);
        }
    }
}
