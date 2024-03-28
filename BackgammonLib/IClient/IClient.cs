namespace Network.Interfaces
{
    public interface IClient
    {
        Task Connect();
        Task CreateRoom(string roomName);
        Task JoinRoom(string roomName);
        Task LeaveRoom(string roomName);
        Task MoveRequest(string request);

    }

}
