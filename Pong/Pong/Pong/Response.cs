using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace PongServer
{
    public class Response
    {
        Request request;
        byte[] bytedata;

        public Response(Request request)
        {
            this.request = request;
            ProcessData();
            Post();

            //request.Handler.Shutdown(SocketShutdown.Both);
            //request.Handler.Close();

            SocketServer.runningConnections--;
        }

        void ProcessData()
        {
            switch (request.Type)
            {
                case RequestType.MoveUp:
                    Player.players[request.PlayerID - 1].Move(-1);
                    bytedata = Encoding.UTF8.GetBytes(Player.GetPositions());
                    break;

                case RequestType.MoveDown:
                    Player.players[request.PlayerID - 1].Move(1);
                    bytedata = Encoding.UTF8.GetBytes(Player.GetPositions());
                    break;

                case RequestType.Pause:
                    break;

                case RequestType.Exit:
                    break;

                case RequestType.RegisterPlayer:
                    bytedata = Encoding.UTF8.GetBytes(request.PlayerID.ToString());
                    break;

                case RequestType.Undefined:
                    bytedata = Encoding.UTF8.GetBytes(Player.GetPositions());
                    break;
            }
        }

        public void Post()
        {
            request.Handler.Send(bytedata);
            Console.WriteLine(Encoding.UTF8.GetString(bytedata));
        }
    }
}
