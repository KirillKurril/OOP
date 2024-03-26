using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Network.Services;
using Newtonsoft.Json;

namespace ClientLogic.Services
{
    public class CommunicationManager
    {

        public bool OnePlayerOnly;
        public MoveVerifier Verifier { get; set; }
        private Client client;

        public List<(int, int)> detailedReport;
        public int CurColor;
        public int MovesCounter;

        public CommunicationManager(string ip, int port, bool onePlayerOnly)
        {
            /*client = new Client(ip, port);
            OnePlayerOnly = onePlayerOnly;
            string json = client.GetResponse();
            var gameData = JsonConvert.DeserializeObject<GameStatusData>(json);
            Refresh(gameData);*/
        }
        private void Refresh(GameStatusData gameData)
        {
            Verifier = new MoveVerifier(gameData.diceValues, gameData.moveValues,
                gameData.curField, gameData.curPlayer, gameData.hatsOffToYou);

            detailedReport.Clear();
                foreach (var cell in gameData.whiteField)
                    detailedReport.Add((cell.GetColor(), cell.GetHeight()));
            }
        }

        
    }
