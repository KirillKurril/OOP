﻿using System.Collections.ObjectModel;
using Network.Interfaces;
using Network.Services.Client;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Entities;

namespace TestClient
{
    internal class Program
    {
        static IClient client;
        static string room;
        static int _color;
        static char _piece;
        static bool myTurn;
        
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
            client.ReceiveGameStatusEvent += GameStatusHandler;
            Task.Run(async () => await client.Connect());
            while(true)
            {
                Console.WriteLine("1 - Создать\n2 - Подключиться\n");
                int choise = int.Parse(Console.ReadLine());
                if (choise == 1)
                {
                    Console.WriteLine("Введите название комнаты:\n");
                    var roomName = Console.ReadLine();
                    Task.Run(async () => await client.CreateRoom(roomName));
                }
                else
                {
                    Console.WriteLine("Введите название комнаты:\n");
                    var roomName = Console.ReadLine();
                    Task.Run(async () => await client.JoinRoom(roomName));
                }
                if (_color != 0)
                    break;
            }
            
            
            while(true)
            {
                if(myTurn)
                {
                    int source= int.Parse(Console.ReadLine());
                    int dstination = int.Parse(Console.ReadLine());
                    Task.Run(async() => await client.MoveRequest(source, dstination)); 
                }
            }
        }

        private static void GameStatusHandler(object sender, GameStatusData data)
        {
            Console.WriteLine($"Dice calues: \n");
            foreach (var diceValue in data.DiceValues)
                Console.WriteLine(diceValue);
            
            Console.WriteLine($"Score: {data.Score}");

            myTurn = _color == data.MoveColor;

            int counter = 0;
            foreach( var pair in data.ExtraStatus)
            {
                Console.WriteLine($"[{counter}] ");
                for (int i = pair.Item1; i > 0; i--)
                    Console.Write(_piece);
            }
    }

        private static void RoomCompleteHandler(object? sender, EventArgs e)
        {
            Console.WriteLine($"Комната {client._roomName} успешно создана\n");
            client.RequestColor();
        }

        private static void RoomConnectionHandler(object sender, bool answer, string message)
            => Console.WriteLine(message);

        private static void ConnectionStatusHandler(object sender, string message)
            => Console.WriteLine(message);

        private static void ColorResponseHandler(object sender, int color)
        {
            _color = color;
            Console.WriteLine($"Полученный цвет: {color}");
            if (color == 1)
                _piece = '○';
            else
                _piece = '●';
        }
    }
}
