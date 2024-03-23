using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Network.Entities
{
    public class Client
    {
        string ip;
        int port;
        public Client(string ip, int port) 
            => (this.ip, this.port) = (ip, port);

        public void Connect()
        {

        }
        public string Post()
        {
            return string.Empty;
        }
        public string Get()
        {
            return string.Empty;
        }

        public void Disconnect()
        {

        }
       
    }
}
