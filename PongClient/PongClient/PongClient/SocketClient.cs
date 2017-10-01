using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PongClient {
    public class SocketClient
    {
        public static bool FrameUpdate = false;

        static Socket sender;
        static byte[] bytes = new byte[1024];

        static IPEndPoint remoteEP;

        static int MaxDelay = 100;
        static float delay = 0;

        public static double SocketID;
        public static int PlayerID = 0;

        public static void StartClient()
        {
            Random random = new Random();
            SocketID = random.NextDouble();
            Console.WriteLine("SocketID: " + SocketID);

            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            remoteEP = new IPEndPoint(ipAddress, 11000);

            Thread write = new Thread(WriteThread);
            write.IsBackground = false;
            write.Start();

            Thread read = new Thread(ReadThread);
            read.IsBackground = true;
            read.Start();

            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP);
        }

        static void WriteThread()
        {
            while (true)
            {
                try
                {
                    if (FrameUpdate)
                    {
                        Keys[] pressedKeys = Pong.currKeyState.GetPressedKeys();
                        string data = "";

                        if (PlayerID != 0)
                        {
                            if (pressedKeys.Length > 0 && Pong.prevKeyState != Pong.currKeyState)
                            {
                                data = PlayerID + Pong.currKeyState.GetPressedKeys()[0].ToString();
                                delay = 0;
                            }
                            else if (delay >= MaxDelay)
                            {
                                data = PlayerID.ToString();
                                delay = 0;
                            }
                            else
                            {
                                delay += Pong.deltaTime;
                                FrameUpdate = false;
                                continue;
                            }
                        }
                        else
                        {
                            data = "?" + SocketID;
                        }

                        sender.Send(Encoding.UTF8.GetBytes(data));

                        FrameUpdate = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void ReadThread()
        {
            while (true)
            {
                if (!FrameUpdate && sender != null)
                {
                    int a = sender.Receive(bytes);

                    string data = Encoding.UTF8.GetString(bytes, 0, a);

                    DataHandle dataHandler = new DataHandle(sender, data);
                }
            }
        }
    }
}

