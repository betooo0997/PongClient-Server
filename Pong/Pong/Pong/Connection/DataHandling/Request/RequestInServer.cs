using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pong
{
    public class RequestInServer : Request
    {
        /// <summary>
        /// The PlayerID of the incoming Request.
        /// </summary>
        public int PlayerID;

        DataHandler datahandler;

        /// <summary>
        /// The type of the Request.
        /// </summary>
        public RequestType Type { get; private set; }

        /// <summary>
        /// The enum which describes which type the request is about.
        /// </summary>
        public enum RequestType
        {
            MoveUp,
            MoveDown,
            Pause,
            Exit,
            RegisterPlayer,
            Undefined
        }

        /// <summary>
        /// The class constructor.
        /// </summary>
        public RequestInServer(string data, DataHandler datahandler)
        {
            PlayerID = 0;
            this.datahandler = datahandler;

            GetInformation(data);
        }

        /// <summary>
        /// Analyzes the incoming data.
        /// </summary>
        /// <param name="data"></param>
        void GetInformation(string data)
        {
            if (data.Length < 1)
                return;

            Console.WriteLine("DATA INCOME: " + data);

            int.TryParse(data.First().ToString(), out PlayerID);

            if (PlayerID == 0)
            {
                //try
                //{
                if (!PongConnection.CheckPassword(data.Replace("?", "")))
                {
                    Console.WriteLine("Inputted password is wrong! " + data);
                    datahandler.connection.socket.Send(Encoding.UTF8.GetBytes("CLOSE"));
                    datahandler.connection.socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                    datahandler.connection.socket.Close();
                    datahandler.connection.correctPassword = false;
                    datahandler.connection.initalized = true;
                }
                else
                {
                    datahandler.connection.AddToArray();
                    datahandler.connection.correctPassword = true;
                    datahandler.connection.socket.Send(Encoding.UTF8.GetBytes("ACCEPTED"));
                    ResponseExpected = true;

                    Type = RequestType.RegisterPlayer;
                    PlayerID = PongConnection.RegisteredClientIDs.Length + 1;

                    double[] temp = PongConnection.RegisteredClientIDs;
                    double[] newArray = new double[temp.Length + 1];

                    for (int x = 0; x < newArray.Length; x++)
                    {
                        if (x < newArray.Length - 1)
                            newArray[x] = temp[x];
                        else
                            newArray[x] = newArray.Length;
                    }

                    PongConnection.RegisteredClientIDs = newArray;
                    datahandler.connection.initalized = true;
                }

                return;
            }

            char Command = data.ToUpper().Last();

            switch (Command)
            {
                case 'W':
                    Type = RequestType.MoveUp;
                    ResponseExpected = true;
                    break;

                case 'S':
                    Type = RequestType.MoveDown;
                    ResponseExpected = true;
                    break;

                case 'P':
                    Type = RequestType.Pause;
                    break;

                case 'E':
                    Type = RequestType.Exit;
                    break;

                default:
                    Type = RequestType.Undefined;
                    ResponseExpected = true;
                    Console.WriteLine("UNDEFINED");
                    break;
            }
        }
    }
}
