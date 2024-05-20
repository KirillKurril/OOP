using Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using ServerDB.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities.GameServices
{
    public class NetGame
    {
        public int Id { get; set; }
        public int CurPlayerInd { get; set; }
        public List<Player> Players { get; set; }
        [NotMapped]
        public List<Cell> СurField
            => CurPlayerInd == 0 ? Board.WhiteField : Board.BlackField;
        public GameBoard Board { get; set; }
        public List<int> Status { get; set; } = [];
        public List<int> DiceValues { get; set; } = [];
        public List<int> MoveValues { get; set; } = [];   
        public bool HatsOffToYou { get; set; }
        public string RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public NetGame() { }
        public NetGame(int init)
        {
            Board = new GameBoard();
            Players = new List<Player>(2);
            Players.Add(new Player(Colors.White));
            Players.Add(new Player(Colors.Black));
            CurPlayerInd = 0;

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
            bool rigthColor = Status[startPosition] == Players[CurPlayerInd].Color;
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
                if (СurField[i].GetColor() == Players[CurPlayerInd].Color)
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
            bool capturedByFriendlyUnit = Status[destinatioin] == Players[CurPlayerInd].Color;
            
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
                CurPlayerInd = Players[CurPlayerInd].Color == Colors.White ? Colors.Black : Colors.White;
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
            return throwDices.Count != 0 && Players[CurPlayerInd].ReachedHome;
        }
        public List<int> GetStatus()
            => Status;
        public GameStatusData GetGameStatus()
        {
            GameStatusData data = new GameStatusData(
                Status,
                DiceValues,
                MoveValues,
                Players[CurPlayerInd].ReachedHome,
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
            => Players[CurPlayerInd].Color;
        public bool GetPlayerStatus()
            => Players[CurPlayerInd].ReachedHome;
        public int GetCurColor()
            => Players[CurPlayerInd].Color;
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
            => Players[CurPlayerInd].SafeMode = Status.Skip(18).All(position => position != Players[CurPlayerInd].Color);
        private void StatusRefresh()
        {
            Status.Clear();
            for (int i = 0; i < СurField.Count; i++)
                Status.Add(СurField[i].GetColor());
        }
        private void ReachedHomeRefresh()
            => Players[CurPlayerInd].ReachedHome = Status.Take(18).All(position => position != Players[CurPlayerInd].Color);
        private void ScoreRefresh(int modul)
            => Players[CurPlayerInd].Score -= modul;
        public bool CheckEndGame()
            => Players[CurPlayerInd].Score == 0;
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
