using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PongServer
{
    public class Request
    {
        public static bool ArrayLock = false;

        public Socket Handler { get; private set; }
        public Thread requestThread { get; private set; }

        public int PlayerID;
        public RequestType Type { get; private set; }

        byte[] bytes;

        int a;

        public Request(Socket newSocket, byte[] bytes, int a)
        {
            PlayerID = 0;
            Handler = newSocket;
            this.bytes = bytes;
            this.a = a;

            requestThread = new Thread(HandleData);
            requestThread.IsBackground = false;
            requestThread.Start();
        }

        void HandleData()
        {
            string data = Encoding.UTF8.GetString(bytes, 0, a);

            GetInformation(data);

            Response response = new Response(this);

            Console.WriteLine("Connection closed, total Connections: " + SocketServer.runningConnections);
        }

        void GetInformation(string data)
        {
            Console.WriteLine(data);

            int.TryParse(data.First().ToString(), out PlayerID);

            if (PlayerID == 0)
            {
                Type = RequestType.RegisterPlayer;
                PlayerID = SocketServer.RegisteredClientIDs.Length + 1;

                double[] temp = SocketServer.RegisteredClientIDs;
                double[] newArray = new double[temp.Length + 1];

                for(int x = 0; x < newArray.Length; x++)
                {
                    if (x < newArray.Length - 1)
                        newArray[x] = temp[x];
                    else
                        newArray[x] = double.Parse(data.Substring(1));
                }

                SocketServer.RegisteredClientIDs = newArray;
                Console.WriteLine(newArray[newArray.Length - 1]);
                return;
            }

            char Command = data.ToUpper().Last();

            switch (Command)
            {
                case 'W':
                    Type = RequestType.MoveUp;
                    break;

                case 'S':
                    Type = RequestType.MoveDown;
                    break;

                case 'P':
                    Type = RequestType.Pause;
                    break;

                case 'E':
                    Type = RequestType.Exit;
                    break;

                default:
                    Type = RequestType.Undefined;
                    break;
            }
        }
    }

    public enum RequestType
    {
        MoveUp,
        MoveDown,
        Pause,
        Exit,
        RegisterPlayer,
        Undefined
    }
}

//void IncreaseArray()
//{
//    Socket[] sTemp = Handlers;
//    Thread[] tTemp = Reads;

//    Handlers = new Socket[sTemp.Length + 1];
//    Reads = new Thread[tTemp.Length + 1];

//    for (int x = 0; x < Handlers.Length; x++)
//    {
//        if (x < sTemp.Length - 1)
//        {
//            Handlers[x] = sTemp[x];
//            Reads[x] = tTemp[x];
//        }
//        else
//        {
//            Handlers[x] = Handler;
//            Reads[x] = requestThread;
//        }
//    }
//}

//public void DecreaseArray()
//{
//    Socket[] sTemp = Handlers;
//    Thread[] tTemp = Reads;

//    Handlers = new Socket[sTemp.Length - 1];
//    Reads = new Thread[tTemp.Length - 1];

//    bool skipped = false;

//    for (int x = 0; x < Handlers.Length; x++)
//    {
//        if (!skipped)
//        {
//            if (sTemp[x] == Handler && tTemp[x] == requestThread)
//                skipped = true;
//            else
//            {
//                Handlers[x] = sTemp[x];
//                Reads[x] = tTemp[x];
//            }
//        }
//        else
//        {
//            Handlers[x - 1] = sTemp[x];
//            Reads[x - 1] = tTemp[x];
//        }
//    }

//    Handler = null;
//}
