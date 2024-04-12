using Entities;
using ServerDB.Models;

namespace ServerDB.Repositories
{
    public interface IRoomRepository : IDisposable
    {
        IEnumerable<Room> GetRooms();
        GameStatusData? GetStatus(string roomName);
        int? GetCurColor(string roomName);
        Room? GetRoom(string roomName);
        bool Contains(string roomName);
        bool ContainsPlayer(string roomName, string playerConnectionId);
        bool IsFull(string roomName);
        bool IsEmpty(string roomName);
        void Add(string roomName);
        void AddPlayer(string roomName, string playerName);
        void Remove(string roomName);
        void RemovePlayer(string roomName, string playerName);
        void MakeMove(string roomName, int source, int dstination);
        List<string>? GetPlayers(string roomName);
    }
}
