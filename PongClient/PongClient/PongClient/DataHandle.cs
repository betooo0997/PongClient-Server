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
        byte[] bytes;
        int a;
        Socket sender;

        public DataHandle(Socket sender, byte[] bytes, int a)
        {
            this.a = a;
            this.bytes = bytes;
            this.sender = sender;

            Thread newThread = new Thread(() => ProcessData());
            newThread.Start();
        }

        void ProcessData(string data = "")
        {
            try
            {
                if (data == "")
                    data = Encoding.UTF8.GetString(bytes, 0, a);

                string[] commands = data.Split('!');

                if (commands.Length > 2)
                {
                    Console.WriteLine("ERROR; COMMANDS HAVE BEEN MERGED:");
                    int comnum = 1;
                    foreach (string command in commands)
                    {
                        ProcessData(command);
                        Console.WriteLine(comnum + ": " + command);
                        comnum++;
                    }
                    return;
                }

                data = data.Replace("!", "");

                string[] tokens = data.Split(' ');

                if (data.StartsWith("B"))
                {
                    Vector2 ballPosition = new Vector2(float.Parse(tokens[1]), float.Parse(tokens[2]));
                    Vector2 ballDirection = new Vector2(float.Parse(tokens[3]), float.Parse(tokens[4]));
                    Ball.Position = ballPosition;
                    Ball.directionVector = ballDirection;
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
