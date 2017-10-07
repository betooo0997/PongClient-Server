using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Pong
{
    public class DataHandler
    {
        string data;
        public ConnectionHandler connection { get; private set; }
        byte[] bytes;
        int a;

        public DataHandler(ConnectionHandler connection, byte[] bytes, int a)
        {
            this.connection = connection;
            this.bytes = bytes;
            this.a = a;

            Thread handleData;

            if (PongConnection.PlayerID == -1)
                handleData = new Thread(HandleDataInServer);
            else
                handleData = new Thread(HandleDataInClient);

            handleData.IsBackground = false;
            handleData.Start();
        }

        void HandleDataInClient()
        {
            RequestInClient request = new RequestInClient(bytes, a);
            ResponseInClient response = new ResponseInClient(request, this);
            response.Post();

            PongConnection.runningConnections--;
        }

        void HandleDataInServer()
        {
            RequestInServer request = new RequestInServer(bytes, a);
            ResponseInServer response = new ResponseInServer(request, this);
            response.Post();

            PongConnection.runningConnections--;
        }
    }
}
