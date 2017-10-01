using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace PongServer
{
    public class SocketServer
    {
        public int port { get; private set; }

        public static int runningConnections = 0;

        public static double[] RegisteredClientIDs;

        Thread serverMain;

        IPEndPoint localEndPoint;
        IPAddress ipAddress;

        Socket listener;

        bool running = false;

        public SocketServer(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            if (!running)
            {
                serverMain = new Thread(Initialize);
                serverMain.IsBackground = false;
                serverMain.Start();
            }
        }

        void Initialize()
        {
            RegisteredClientIDs = new double[0];

            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            listener.Bind(localEndPoint);
            listener.Listen(100);

            Console.WriteLine("Server IP Adress: " + ipAddress);
            Console.WriteLine("Server Port: " + port);
            Console.WriteLine("\n________________________________________________________________\n");

            StartListening();
        }

        void StartListening()
        {
            while (true)
            {
                Console.WriteLine("\nWaiting for a connection...\n");

                Socket newSocket = listener.Accept();

                HandleClient client = new HandleClient(newSocket);
            }
        }
    }
}
