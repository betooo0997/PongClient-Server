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

        public bool initalized = false;
        public bool correctPassword = false;

        public ConnectionHandler(Socket socket)
        {
            this.socket = socket;

            Thread dataHandling = new Thread(HandleConnectionData);
            dataHandling.Start();
        }

        void HandleConnectionData()
        {
            //try
            //{
                while (true)
                {
                    if (socket.Connected)
                    {
                        byte[] bytes = new byte[1024];
                        int a = socket.Receive(bytes);
                        string data = Encoding.UTF8.GetString(bytes, 0, a);

                        if (data == "CLOSE" || data == "ACCEPTED" || data.First() == '?')
                        {
                            Console.WriteLine("First Income, checking password: " + data);

                            DataHandler dataHandler = new DataHandler(this, data);

                            Console.WriteLine("GOING TO SLEEP");

                            while (!initalized)
                                Thread.Sleep(1);

                            Console.WriteLine("WAKING UP");

                            if (correctPassword)
                                break;
                            else
                                return;
                        }
                    }
                }

                while (true)
                {
                    if (socket.Connected)
                    {
                        byte[] bytes = new byte[1024];
                        int a = socket.Receive(bytes);
                        string data = Encoding.UTF8.GetString(bytes, 0, a);

                        if (data != "")
                        {
                            DataHandler dataHandler = new DataHandler(this, data);
                        }
                    }
                }
            //}
            //catch(SocketException e)
            //{
            //    //Error();
            //    Console.WriteLine(e.Message);
            //}
        }

        void SendBallDataToClient()
        {
            string data = "B " + Ball.Position.X + " " + Ball.Position.Y + " " + Ball.directionVector.X + " " + Ball.directionVector.Y + '!';
            byte[] bytedata = Encoding.UTF8.GetBytes(data);
            socket.Send(bytedata);
        }

        public static void SendBallDataToAllClients()
        {
            try
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
            catch(Exception e)
            {
                //Error();
                Console.WriteLine(e.Message);
            }
        }

        public void AddToArray()
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
                    connections[x] = this;
            }
        }

        public static void SendScoreToAllClients()
        {
            try
            {
                foreach (ConnectionHandler client in connections)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes("S" + Player.players[0].Score + " " + Player.players[1].Score);
                    client.socket.Send(bytes);
                }
            }
            catch (Exception e)
            {
                //Error();
                Console.WriteLine(e.Message);
            }
        }

        public static void Error()
        {
            State_Menu.Singleton.targetState = State_Menu.Singleton;
            Console.WriteLine("ERROR, Targetstate is Menu");
            Pong.targetState = State_Menu.Singleton;
            State_Menu.Singleton.info = "A client has logged out or a connection error \nocurred, you have been disconnected.";

            PongConnection.CloseConnection();
        }
    }
}
