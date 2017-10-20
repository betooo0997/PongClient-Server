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
    /// <summary>
    /// Class that handles the connection to other Pong instances to make the multiplayer experience possible.
    /// </summary>
    public class PongConnection
    {
        /// <summary>
        /// The Player ID of the Pong instance. -1 for the server, 0 if yet unassigned.
        /// </summary>
        public static int PlayerID;

        /// <summary>
        /// The unique Socket ID of the Pong instance.
        /// </summary>
        public static double SocketID;

        /// <summary>
        /// The servers password to join the seesion.
        /// </summary>
        public static string password { get; private set; }

        /// <summary>
        /// The server's port.
        /// </summary>
        public int port { get; private set; }

        /// <summary>
        /// The registered client IDs by the server.
        /// </summary>
        public static double[] RegisteredClientIDs;

        /// <summary>
        /// The IPEndPoint the Socket will be bound to.
        /// </summary>
        static IPEndPoint ipEndPoint;

        /// <summary>
        /// The IP Adress used to create the localEndPoint.
        /// </summary>
        static IPAddress ipAddress;

        /// <summary>
        /// The main socket, creates a new socket for each new connection.
        /// </summary>
        static Socket socket;

        /// <summary>
        /// True if the connection is established (client/server) and the minimum amount of players is fulfilled (if server).
        /// </summary>
        public static bool running = false;

        /// <summary>
        /// The Pong Connection's constructor.
        /// </summary>
        /// <param name="port"></param>
        public PongConnection(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// Called on start.
        /// </summary>
        public void Start()
        {
            if (!running)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Initializes the Pong Connection.
        /// </summary>
        void Initialize()
        {
            RegisteredClientIDs = new double[0];

            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            ipEndPoint = new IPEndPoint(ipAddress, port);
        }

        /// <summary>
        /// Establishes the connection to the server (if client) or lisens to new incomming ones (if server).
        /// </summary>
        /// <param name="server">If the instance shall act as server or not.</param>
        /// <returns></returns>
        public static bool StartListening(bool server, string password)
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
                    Pong.InitializeGame();
                    PongConnection.password = password;
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

                    Console.WriteLine("Sending first message");
                    socket.Send(Encoding.UTF8.GetBytes("?" + State_Menu.Singleton.input));

                    Pong.InitializeGame();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Listens to new incoming clients (aka connections).
        /// </summary>
        static void ListenToClients()
        {
            Console.WriteLine("SERVER");

            while (true)
            {
                Socket newSocket = socket.Accept();

                ConnectionHandler newClient = new ConnectionHandler(newSocket);

                while (!newClient.initalized)
                    Thread.Sleep(1);


                if (ConnectionHandler.connections != null)
                {
                    Console.WriteLine("Connections: " + ConnectionHandler.connections.Length);

                    if (ConnectionHandler.connections.Length > 1)
                    {
                        running = true;
                        ConnectionHandler.SendBallDataToAllClients();
                    }
                }
            }
        }

        /// <summary>
        /// Creates a ConnectionHandler instance that'll handle all server incoming data.
        /// </summary>
        static void ListenToServer()
        {
            running = true;

            Random random = new Random();
            SocketID = random.NextDouble();

            Console.WriteLine("CLIENT");
            ConnectionHandler server = new ConnectionHandler(socket);
        }

        /// <summary>
        /// Sends the user input to the sever, if the client hasn't been registered yet it sends its SocketID and requests the server to do so.
        /// </summary>
        /// <param name="key"></param>
        public static void SyncPlayerPositions(string key)
        {
            Console.WriteLine("KEY PRESSED, SENDING");

            string data = "";

            if (PlayerID != 0)
            {
                data = PlayerID + key;
            }

            if(data != "")
                socket.Send(Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Looks if the given string is equivalent to the servers password.
        /// </summary>
        public static bool CheckPassword(string input)
        {
            if (input == password)
                return true;
            else
                return false;
        }

        public static void CloseConnection()
        {
            if (ConnectionHandler.connections != null)
            {
                foreach (ConnectionHandler connection in ConnectionHandler.connections)
                {
                    try
                    {
                        connection.socket.Shutdown(SocketShutdown.Both);
                        connection.socket.Close();
                    }
                    catch
                    {
                    }
                }
            }

            ConnectionHandler.connections = new ConnectionHandler[0];
            RegisteredClientIDs = new double[0];
            running = false;
        }
    }
}