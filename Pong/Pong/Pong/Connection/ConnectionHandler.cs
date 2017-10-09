using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Pong
{
    public class ConnectionHandler
    {
        public static ConnectionHandler[] connections;

        public Socket socket;

        public ConnectionHandler(Socket socket)
        {
            this.socket = socket;
            AddToArray(this);

            Thread dataHandling = new Thread(HandleConnectionData);
            dataHandling.Start();
        }

        void HandleConnectionData()
        {
            while (true)
            {
                if (socket.Connected)
                {
                    byte[] bytes = new byte[1024];
                    int a = socket.Receive(bytes);

                    DataHandler dataHandler = new DataHandler(this, bytes, a);
                }
            }
        }

        void SendBallDataToClient()
        {
            string data = "B " + Ball.Position.X + " " + Ball.Position.Y + " " + Ball.directionVector.X + " " + Ball.directionVector.Y + '!';
            byte[] bytedata = Encoding.UTF8.GetBytes(data);
            socket.Send(bytedata);
        }

        public static void SendBallDataToAllClients()
        {
            if (connections != null && Ball.timeSinceLastClientSync > Ball.limitTimeSinceSync / 2)
            {
                foreach (ConnectionHandler client in connections)
                {
                    Thread sendBallData = new Thread(client.SendBallDataToClient);
                    sendBallData.Start();
                }

                Console.Write("Ball data sent to all clients.");
                Ball.timeSinceLastClientSync = 0;
            }
        }

        static void AddToArray(ConnectionHandler newClient)
        {
            if (connections == null)
                connections = new ConnectionHandler[0];

            ConnectionHandler[] temp = connections;
            connections = new ConnectionHandler[temp.Length + 1];

            for (int x = 0; x < connections.Length; x++)
            {
                if (x < connections.Length - 1)
                    connections[x] = temp[x];
                else
                    connections[x] = newClient;
            }
        }

        public static void SendScoreToAllClients()
        {
            foreach (ConnectionHandler client in connections)
            {
                byte[] bytes = Encoding.UTF8.GetBytes("S" + Player.players[0].Score + " " + Player.players[1].Score);
                client.socket.Send(bytes);
            }
        }
    }
}
