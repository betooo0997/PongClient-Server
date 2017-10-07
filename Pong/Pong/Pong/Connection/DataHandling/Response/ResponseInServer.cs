using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    public class ResponseInServer : Response
    {
        public ResponseInServer(RequestInServer request, DataHandler dataHandler)
        {
            if (request.ResponseExpected)
            {
                this.request = request;
                this.dataHandler = dataHandler;
                ProcessData(request);
            }
        }

        void ProcessData(RequestInServer request)
        {
            switch (request.Type)
            {
                case RequestInServer.RequestType.MoveUp:
                    Player.players[request.PlayerID - 1].Move(-1);
                    bytedata = Encoding.UTF8.GetBytes(Player.GetPositions() + '!');
                    break;

                case RequestInServer.RequestType.MoveDown:
                    Player.players[request.PlayerID - 1].Move(1);
                    bytedata = Encoding.UTF8.GetBytes(Player.GetPositions() + '!');
                    break;

                case RequestInServer.RequestType.Pause:
                    break;

                case RequestInServer.RequestType.Exit:
                    break;

                case RequestInServer.RequestType.RegisterPlayer:
                    bytedata = Encoding.UTF8.GetBytes('?' + request.PlayerID.ToString() + '!');
                    Console.WriteLine("Bytes gotten");
                    break;

                case RequestInServer.RequestType.Undefined:
                    bytedata = Encoding.UTF8.GetBytes(Player.GetPositions() + '!');
                    break;
            }
        }
    }
}
