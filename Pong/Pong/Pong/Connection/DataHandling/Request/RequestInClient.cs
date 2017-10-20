using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    public class RequestInClient : Request
    {
        public RequestType Type { get; private set; }

        DataHandler datahandler;

        public enum RequestType
        {
            SendingBallPosition,
            Registering,
            Undefined
        }

        public RequestInClient(string data, DataHandler datahandler)
        {
            this.datahandler = datahandler;
            GetInformation(data);
        }

        void GetInformation(string data)
        {
            if (data == "CLOSE")
            {
                Console.WriteLine("Inputted password is wrong! " + data);
                ConnectionHandler.Error();
                State_Menu.Singleton.info = "Wrong password";

                datahandler.connection.correctPassword = false;
                datahandler.connection.initalized = true;
                return;
            }
            else if (data == "ACCEPTED")
            {
                Console.WriteLine("Inputted password is correct! " + data);
                datahandler.connection.AddToArray();
                datahandler.connection.correctPassword = true;
                datahandler.connection.initalized = true;
                State_Menu.Singleton.targetState = State_Playing.Singleton;
                return;
            }

            Console.WriteLine("DATA INCOME: " + data);

            if (data.Split('!').Length > 2)
            {
                Console.WriteLine("Too long, aborting: " + data);
                return;
            }

            data = data.Replace("!", "");
            string[] tokens = data.Split(' ');

            switch (data.First())
            {
                case 'B':
                    Vector2 ballPosition = new Vector2(float.Parse(tokens[1]), float.Parse(tokens[2]));
                    Vector2 ballDirection = new Vector2(float.Parse(tokens[3]), float.Parse(tokens[4]));
                    Ball.Position = ballPosition;
                    Ball.directionVector = ballDirection;
                    return;

                case '?':
                    if (PongConnection.PlayerID == 0)
                        PongConnection.PlayerID = int.Parse(data.Substring(1));
                    return;

                case 'S':
                    tokens[0] = tokens[0].Replace("S", "");
                    Player.players[0].Score = int.Parse(tokens[0]);
                    Player.players[1].Score = int.Parse(tokens[1]);
                    break;

                default:
                    tokens = data.Split(' ');

                    int position0 = int.Parse(tokens[0]);
                    int position1 = int.Parse(tokens[1]);

                    Player.players[0].Position = new Vector2(Player.players[0].Position.X, position0);
                    Player.players[1].Position = new Vector2(Player.players[1].Position.X, position1);
                    break;
            }

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }
    }
}
