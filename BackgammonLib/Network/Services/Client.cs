using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Network.Services;
using Newtonsoft.Json;

namespace Network.Services
{
    public class Clientus
    {
        private int Port;
        private string IP;
        private NetworkStream? stream;
        public Clientus(string ip = "127.0.0.1", int port = 1234) 
            => (IP, Port) = (ip, port);

        public async Task<bool> SendMove(string json)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(json);
                await stream.WriteAsync(data, 0, data.Length);
                Debug.WriteLine("Move sent to the server.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return false;
            }
            return true;
        }
        public async Task<string>? GetResponse()
        {
            string responseData;
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                responseData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
            return responseData;
        }



        public void Disconnect()
        {
            stream.Close();
            Debug.WriteLine("Connection closed.");
        }
       
    }
}


//Вставить в клиент 
/*protected override void OnClosing(CancelEventArgs e)
{
    Clientus.CloseConnection();
    base.OnClosing(e);
}*/
