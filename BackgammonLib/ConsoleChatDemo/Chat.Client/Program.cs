// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.SignalR.Client;
Console.WriteLine("Hello, World!");
// Url сервера (см. файл Properties/LaunchSettings.json)
var url = "https://localhost:7094/chat";

// Создать объект подключения к серверу
var hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();
// что делать, если пришло от сервера сообщение "ReceiveMessage"
hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
{
    Console.WriteLine($"{user}: {message}");    
});

// соединиться с сервером
await hubConnection.StartAsync();
// ввести имя 
Console.WriteLine("Ваше имя?");
var name = Console.ReadLine();

while(true)
{
    // ввести сообщение
    Console.Write("---> ");
    var message = Console.ReadLine();
    // отправить сообщение на сервер
    await hubConnection.InvokeAsync("SendMessage", message, name);
}
