using Entities.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace Network.Interfaces
{
    public interface IClient
    {
        public string _roomName { get;}
        public string URL { get; }
        public HubConnection hubConnection { get; }

        delegate void ReceiveGameStatusDelegate(object sender, GameStatusData data);
        delegate void CreateRoomResponseDelegate(object sender, bool answer, string message);
        delegate void ConnectionStatusDelegate(object sender, string status);

        event ReceiveGameStatusDelegate ReceiveGameStatusEvent;
        event CreateRoomResponseDelegate CreateRoomResponseEvent;
        event CreateRoomResponseDelegate JoinRoomResponseEvent;
        event ConnectionStatusDelegate ConnectionStatusEvent;
        public event EventHandler RoomCompleted;
        public event EventHandler EndGame;
        public event EventHandler<int> ColorResponse;
        Task Connect();
        Task CreateRoom(string roomName);
        Task JoinRoom(string roomName);
        Task MoveRequest(int source, int destination);
        Task RequestColor();
        Task LeaveRoom();
        Task Disconnect();
        void SetURL(string url);

    }

}
