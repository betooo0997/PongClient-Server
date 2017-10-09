using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class PongConnection
    {
        public static int PlayerID;

        static string password;

        public int port { get; private set; }

        public static double[] RegisteredClientIDs;

        Thread pongConnectionMain;

        static IPEndPoint ipEndPoint;
        static IPAddress ipAddress;

        static Socket socket;

        public static bool running = false;

        public static double SocketID;

        public PongConnection(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            if (!running)
            {
                Initialize();
            }
        }

        void Initialize()
        {
            RegisteredClientIDs = new double[0];

            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            ipEndPoint = new IPEndPoint(ipAddress, port);
        }

        public static bool StartListening(bool server)
        {
            // Create a TCP/IP socket.
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Thread pongConnectionMain;

            if (server)
            {
                try
                {
                    socket.Bind(ipEndPoint);
                    socket.Listen(4);

                    PlayerID = -1;
                    pongConnectionMain = new Thread(ListenToClients);
                    pongConnectionMain.IsBackground = false;
                    pongConnectionMain.Start();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    socket.Connect(ipEndPoint);

                    PlayerID = 0;
                    pongConnectionMain = new Thread(ListenToServer);
                    pongConnectionMain.IsBackground = false;
                    pongConnectionMain.Start();
                }
                catch(Exception e)
                {
                    return false;
                }
            }

            return true;
        }

        static void ListenToClients()
        {
            Console.WriteLine("SERVER");

            while (true)
            {
                Console.WriteLine("\nWaiting for a connection...\n");

                Socket newSocket = socket.Accept();

                Console.WriteLine("NEW CLIENT");

                ConnectionHandler newClient = new ConnectionHandler(newSocket);

                if (ConnectionHandler.connections.Length >= 2)
                {
                    Ball.paused = false;
                    running = true;
                    Console.WriteLine("GAME RUNNING");
                }
            }
        }

        static void ListenToServer()
        {
            Ball.paused = false;
            running = true;

            Random random = new Random();
            SocketID = random.NextDouble();

            Console.WriteLine("CLIENT");
            ConnectionHandler server = new ConnectionHandler(socket);
        }

        public static void SendPlayerInputToServer(string key = null)
        {
            try
            {
                string data = "";

                if (PlayerID != 0)
                {
                    if (key != null)
                    {
                        data = PlayerID + key;
                    }
                    else
                    {
                        data = PlayerID.ToString();
                    }
                }
                else
                {
                    data = "?" + SocketID;
                }

                socket.Send(Encoding.UTF8.GetBytes(data));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool CheckPassword(string input)
        {
            if (input == password)
                return true;
            else
                return false;
        }
    }
}