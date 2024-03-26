using Microsoft.AspNetCore.SignalR;

namespace Chat.Server
{
    /// <summary>
    /// Хаб сервиса Chat
    /// </summary>
    public class ChatHub : Hub
    {
        // Сообщение от клиента
        public async Task SendMessage(string user, string message)
        {
            // Отправить сообщение всем клиентам
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
