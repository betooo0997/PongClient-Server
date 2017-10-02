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
        public static HandleClient[] clients;

        Socket handler;

        public HandleClient(Socket handler)
        {
            this.handler = handler;
            AddToArray(this);

            Thread datahandling = new Thread(HandleClientData);
            datahandling.Start();
        }

        static void AddToArray(HandleClient newClient)
        {
            if (clients == null)
                clients = new HandleClient[0];

            HandleClient[] temp = clients;
            clients = new HandleClient[temp.Length + 1];

            for (int x = 0; x < clients.Length; x++)
            {
                if (x < clients.Length - 1)
                    clients[x] = temp[x];
                else
                    clients[x] = newClient;
            }
        }

        void HandleClientData()
        {
            while (true)
            {
                if (handler.Connected)
                {
                    byte[] bytes = new byte[1024];
                    int a = handler.Receive(bytes);

                    Request request = new Request(handler, bytes, a);
                }
            }
        }

        public static void SendBallDataToAllClients()
        {
            if (clients != null && Ball.timeSinceLastClientSync > Ball.limitTimeSinceSync / 2)
            {
                foreach (HandleClient client in clients)
                {
                    Thread sendBallData = new Thread(client.SendBallData);
                    sendBallData.Start();
                }

                Console.Write("Ball data sent to all clients.");
                Ball.timeSinceLastClientSync = 0;
            }
        }

        void SendBallData()
        {
            string data = "B " + Ball.Position.X + " " + Ball.Position.Y + " " + Ball.directionVector.X + " " + Ball.directionVector.Y + '!';
            byte[] bytedata = Encoding.UTF8.GetBytes(data);
            handler.Send(bytedata);
        }
    }
}
