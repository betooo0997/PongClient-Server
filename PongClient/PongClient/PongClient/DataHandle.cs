using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PongClient
{
    public class DataHandle
    {
        string data;
        Socket sender;

        public DataHandle(Socket sender, string data)
        {
            this.data = data;
            this.sender = sender;

            Thread newThread = new Thread(ProcessData);
            newThread.Start();
        }

        void ProcessData()
        {
            if (data.Length <= 2 && SocketClient.PlayerID != 0)
                return;

            Console.WriteLine(data);

            if (SocketClient.PlayerID == 0)
            {
                SocketClient.PlayerID = int.Parse(data);
                return;
            }

            string[] tokens = data.Split(' ');

            if (tokens[0] == "B")
            {

            }
            else
            {
                int position0 = int.Parse(tokens[0]);
                int position1 = int.Parse(tokens[1]);

                Player.players[0].Position = new Vector2(Player.players[0].Position.X, position0);
                Player.players[1].Position = new Vector2(Player.players[1].Position.X, position1);
            }
        }
    }
}
