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
            string[] tokens = data.Split(' ');

            if (data.StartsWith("B"))
            {
                Vector2 ballPosition = new Vector2(float.Parse(tokens[1]), float.Parse(tokens[2]));
                Pong.ball.Position = ballPosition;
                return;
            }

            if (data.Length <= 2 && SocketClient.PlayerID != 0)
                return;

            Console.WriteLine(data);

            if (SocketClient.PlayerID == 0)
            {
                SocketClient.PlayerID = int.Parse(data);
                return;
            }

            tokens = data.Split(' ');

            int position0 = int.Parse(tokens[0]);
            int position1 = int.Parse(tokens[1]);

            Player.players[0].Position = new Vector2(Player.players[0].Position.X, position0);
            Player.players[1].Position = new Vector2(Player.players[1].Position.X, position1);
        }
    }
}
