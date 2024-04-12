using BackgammonLogic;
using Entities;
using ServerDB.DBContext;
using ServerDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var room = new Room(roomName);
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
            => db.Rooms.Find(roomName);
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
                room.Game.Move(source, destination);
                db.SaveChanges();
            }
        }
        public GameStatusData? GetStatus(string roomName)
        {
            var room = GetRoom(roomName);
            return room != null ? room.Game.GetGameStatus() : null;
        }
        public int? GetCurColor(string roomName)
        {
            var room = GetRoom(roomName);
            return room != null ? room.Game.GetCurColor() : null;
        }
        public List<string>? GetPlayers(string roomName)
        {
            var room = GetRoom(roomName);
            return room != null ? room.Players : null;
        }
    }
}
