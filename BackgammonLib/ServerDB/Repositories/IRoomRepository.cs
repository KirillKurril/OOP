using Entities;
using ServerDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDB.Repositories
{
    public interface IRoomRepository : IDisposable
    {
        IEnumerable<Room> GetRooms();
        GameStatusData? GetStatus(string roomName);
        int? GetCurColor(string roomName);
        Room? GetRoom(string name);
        bool Contains(string name);
        bool ContainsPlayer(string name, string playerConnectionId);
        bool IsFull(string name);
        bool IsEmpty(string name);
        void Add(Room item);
        void Add(string name);
        void Remove(string name);
        void MakeMove(string name, int source, int dstination);
    }
}
