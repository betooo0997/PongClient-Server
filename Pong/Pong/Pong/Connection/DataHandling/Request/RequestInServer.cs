using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    public class RequestInServer : Request
    {
        public int PlayerID;

        public RequestType Type { get; private set; }

        public enum RequestType
        {
            MoveUp,
            MoveDown,
            Pause,
            Exit,
            RegisterPlayer,
            Undefined
        }

        public RequestInServer(byte[] bytes, int a)
        {
            PlayerID = 0;
            this.bytes = bytes;
            this.a = a;

            string data = Encoding.UTF8.GetString(bytes, 0, a);
            GetInformation(data);
        }

        void GetInformation(string data)
        {
            Console.WriteLine("DATA INCOME: " + data);

            int.TryParse(data.First().ToString(), out PlayerID);

            if (PlayerID == 0)
            {
                try
                {
                    Type = RequestType.RegisterPlayer;
                    PlayerID = PongConnection.RegisteredClientIDs.Length + 1;

                    double[] temp = PongConnection.RegisteredClientIDs;
                    double[] newArray = new double[temp.Length + 1];

                    for (int x = 0; x < newArray.Length; x++)
                    {
                        if (x < newArray.Length - 1)
                            newArray[x] = temp[x];
                        else
                            newArray[x] = double.Parse(data.Substring(1));
                    }

                    PongConnection.RegisteredClientIDs = newArray;
                    ResponseExpected = true;
                }
                catch { }
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
