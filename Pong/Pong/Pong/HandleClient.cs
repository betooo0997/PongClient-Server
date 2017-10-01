using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PongServer
{
    class HandleClient
    {
        Socket handler;
        float timer;

        public HandleClient(Socket handler)
        {
            this.handler = handler;

            Thread ballSending = new Thread(SendBallData);
            ballSending.Start();
            HandleClientData();
        }

        void HandleClientData()
        {
            while (true)
            {
                if (!handler.Connected)
                    return;

                byte[] bytes = new byte[1024];

                int a = handler.Receive(bytes);

                Request request = new Request(handler, bytes, a);
            }
        }

        void SendBallData()
        {
            Thread.Sleep(2000);
            while (true)
            {
                if (Pong.frameUpdate)
                {
                    Thread.Sleep(50);

                    Pong.frameUpdate = false;
                    string data = "B " + Ball.Position.X + " " + Ball.Position.Y;
                    byte[] bytedata = Encoding.UTF8.GetBytes(data);
                    handler.Send(bytedata);
                    Console.Write("B");
                }
            }
        }
    }
}
