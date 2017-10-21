using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Pong
{
    /// <summary>
    /// Class that manages the Responses inside the server.
    /// </summary>
    public class ResponseInServer : Response
    {
        /// <summary>
        /// True if the Response should be sent to all clients.
        /// </summary>
        public bool sendToAllClients = false;

        /// <summary>
        /// The class constructor.
        /// </summary>
        public ResponseInServer(RequestInServer request, DataHandler dataHandler)
        {
            if (request.ResponseExpected)
            {
                this.request = request;
                this.dataHandler = dataHandler;
                ProcessData(request);
            }
        }

        /// <summary>
        /// Creates a response based on the analyzed data by the Request instance.
        /// </summary>
        void ProcessData(RequestInServer request)
        {
            switch (request.Type)
            {
                case RequestInServer.RequestType.MoveUp:
                    Player.players[request.PlayerID - 1].Move(-1);
                    bytedata = Encoding.UTF8.GetBytes(Player.GetPositions() + '!');
                    sendToAllClients = true;
                    break;

                case RequestInServer.RequestType.MoveDown:
                    Player.players[request.PlayerID - 1].Move(1);
                    bytedata = Encoding.UTF8.GetBytes(Player.GetPositions() + '!');
                    sendToAllClients = true;
                    break;

                case RequestInServer.RequestType.Pause:
                    break;

                case RequestInServer.RequestType.Exit:
                    break;

                case RequestInServer.RequestType.RegisterPlayer:

                    if (dataHandler.connection.correctPassword)
                    {
                        dataHandler.connection.socket.Send(Encoding.UTF8.GetBytes("ACCEPTED"));
                        Thread.Sleep(10);
                        bytedata = Encoding.UTF8.GetBytes('?' + request.PlayerID.ToString() + '!');
                    }
                    else
                    {
                        dataHandler.connection.socket.Send(Encoding.UTF8.GetBytes("REJECTED"));
                        dataHandler.connection.socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                        dataHandler.connection.socket.Close();
                    }

                        Console.WriteLine("Bytes gotten");
                    break;

                case RequestInServer.RequestType.Undefined:
                    bytedata = Encoding.UTF8.GetBytes(Player.GetPositions() + '!');
                    Console.WriteLine("Bytes gotten");
                    break;
            }
        }
    }
}
