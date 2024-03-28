using Entities;

namespace Network.Interfaces
{
    public interface IClient
    {
        delegate void ReceiveGameStatusDelegate(object sender, GameStatusData data);
        delegate void CreateRoomResponseDelegate(object sender, bool answer, string message);
        delegate void ConnectionStatusDelegate(object sender, string status);

        event ReceiveGameStatusDelegate ReceiveGameStatusEvent;
        event CreateRoomResponseDelegate CreateRoomResponseEvent;
        event CreateRoomResponseDelegate JoinRoomResponseEvent;
        event ConnectionStatusDelegate ConnectionStatusEvent;
        Task Connect();
        Task CreateRoom(string roomName);
        Task JoinRoom(string roomName);
        Task LeaveRoom();
        Task MoveRequest(string request);
        void SetURL(string url);

    }

}
