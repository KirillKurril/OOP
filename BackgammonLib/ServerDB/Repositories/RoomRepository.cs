using Entities.Models;
using Microsoft.EntityFrameworkCore;
using ServerDB.DBContext;
using ServerDB.Models;

namespace ServerDB.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private RoomsDBContext db;
        public RoomRepository(RoomsDBContext db) 
        {
            this.db = db;
        }
        public bool Contains(string roomName)
            =>  db.Rooms.Find(roomName) != null;
        public bool ContainsPlayer(string roomName, string playerConnectionId)
        {
            var room = GetRoom(roomName);
            return room != null ? room.Players.Contains(playerConnectionId) : false;
        }
        public void Add(string roomName)
        {
            var room = Room.CreateRoom(roomName);
            db.Rooms.Add(room);
            db.SaveChanges();
        }
        public void AddPlayer(string roomName, string playerName)
        {
            var room = db.Rooms.Find(roomName);

            if (room != null)
            {
                room.Players.Add(playerName);
                db.SaveChanges();
            }
        }
        public void Remove(string roomName)
        {
            var removableItem = db.Rooms.Find(roomName);
            
            if (removableItem != null)
            {
                db.Rooms.Remove(removableItem);
                db.SaveChanges();
            }
        }
        public void RemovePlayer(string roomName, string playerName)
        {
            var room = db.Rooms.Find(roomName);

            if (room != null)
            {
                room.Players.Remove(playerName);
                db.SaveChanges();
            }
        }
        public void Dispose()
            => db.Dispose();
        public Room? GetRoom(string roomName)
        {
            return db.Rooms
                .Include(r => r.NetGame)
                    .ThenInclude(ng => ng.Board)
                .Include(r => r.NetGame)
                    .ThenInclude(ng => ng.Players)
                .FirstOrDefault(r => r.Id == roomName);
        }
        public IEnumerable<Room> GetRooms()
            => db.Rooms.ToList();
        public bool IsEmpty(string roomName)
        {
            var room = GetRoom(roomName);
            return room != null ? room.Players.Count() == 0 : false;
        }
        public bool IsFull(string roomName)
        {
            var room = GetRoom(roomName);
            return room != null ? room.Players.Count() == 2 : false;
        }
        public void MakeMove(string roomName, int source, int destination)
        {
            var room = GetRoom(roomName);
            
            if(room != null)
            { 
                room.NetGame.Move(source, destination);
                db.SaveChanges();
            }
        }
        public GameStatusData? GetStatus(string roomName)
        {
            var room = GetRoom(roomName);
            Console.WriteLine(room);
            return room != null ? room.NetGame.GetGameStatus() : null;
        }
        public int? GetCurColor(string roomName)
        {
            var room = GetRoom(roomName);
            return room != null ? room.NetGame.GetCurColor() : null;
        }
        public List<string>? GetPlayers(string roomName)
        {
            var room = GetRoom(roomName);
            return room != null ? room.Players : null;
        }
    }
}
