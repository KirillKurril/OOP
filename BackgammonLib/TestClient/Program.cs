using System.Collections.ObjectModel;
using Network.Interfaces;
using Network.Services.Client;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Entities.Models;

namespace TestClient
{
    internal class Program
    {
        static IClient client;
        static int _color;
        static bool myTurn;
        static GameStatusData gd;

        static void Main(string[] args)
        {
            _color = 0;
            client = new Client();
            client.SetURL("https://localhost:7250/game");
            client.ConnectionStatusEvent += ConnectionStatusHandler;
            client.CreateRoomResponseEvent += RoomConnectionHandler;
            client.JoinRoomResponseEvent += RoomConnectionHandler;
            client.RoomCompleted += RoomCompleteHandler;
            client.ColorResponse += ColorResponseHandler;
            client.ReceiveGameStatusEvent += ReceiveGameStatusHandler;
            Task.Run(async () => await client.Connect());
            while (true)
            {
                if (_color != 0)
                    break;

                Console.WriteLine("1 - Создать\n2 - Подключиться\n");
                int choise = int.Parse(Console.ReadLine());
                if (choise == 1)
                {
                    Console.WriteLine("Введите название комнаты:\n");
                    var roomName = Console.ReadLine();
                    Task.Run(async () => await client.CreateRoom(roomName));
                }
                else if (choise == 2)
                {
                    Console.WriteLine("Введите название комнаты:\n");
                    var roomName = Console.ReadLine();
                    Task.Run(async () => await client.JoinRoom(roomName));
                }
            }
        }
        static void MakeTurn()
        {
            while(true)
                if (_color == gd.MoveColor && gd.DiceValues.Count != 0)
                {
                    Console.WriteLine("Теперь ваш ход ^^");
                    Console.WriteLine("Введите позицию первой шашки");
                    int source = int.Parse(Console.ReadLine());
                    Console.WriteLine("Введите позицию второй шашки");
                    int dstination = int.Parse(Console.ReadLine());
                    Task.Run(async () => await client.MoveRequest(source, dstination));
                }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        private static void ReceiveGameStatusHandler(object sender, GameStatusData data)
        {
            Console.WriteLine(data);
            gd = data;
            MakeTurn();
        }

        private static void RoomCompleteHandler(object? sender, EventArgs e)
        {
            Console.WriteLine($"Комната {client._roomName} успешно создана\n");
            Console.WriteLine("Выполняется переход на страницу игры и запрашивается цвет");
            Console.WriteLine("Вы запросили цвет");
            client.RequestColor();
        }

        private static void RoomConnectionHandler(object sender, bool answer, string message)
            => Console.WriteLine(message);

        private static void ConnectionStatusHandler(object sender, string message)
            => Console.WriteLine(message);

        private static void ColorResponseHandler(object sender, int color)
        {
            if (color == 0)
                Console.WriteLine("!!!Ошибка раздачи цвета!!!");
            else
                _color = color;
            Console.WriteLine($"Полученный цвет: {color}");
        }
    }
}
