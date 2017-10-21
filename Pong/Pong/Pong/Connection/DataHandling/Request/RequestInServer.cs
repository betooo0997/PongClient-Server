using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pong
{
    /// <summary>
    /// Class that manages the Requests inside the server.
    /// </summary>
    public class RequestInServer : Request
    {
        /// <summary>
        /// The PlayerID of the incoming Request.
        /// </summary>
        public int PlayerID;

        DataHandler dataHandler;

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
        public RequestInServer(string data, DataHandler dataHandler)
        {
            PlayerID = 0;
            this.dataHandler = dataHandler;

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
                Type = RequestType.RegisterPlayer;
                ResponseExpected = true;

                if (!PongConnection.CheckPassword(data.Replace("?", "")))
                {
                    Console.WriteLine("Inputted password is wrong! " + data);
                    dataHandler.connection.correctPassword = false;
                }
                else
                {
                    dataHandler.connection.AddToArray();
                    dataHandler.connection.correctPassword = true;

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
                }
                dataHandler.connection.initalized = true;

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
