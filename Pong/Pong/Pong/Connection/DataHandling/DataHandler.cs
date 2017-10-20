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


        public DataHandler(ConnectionHandler connection, string data)
        {
            this.connection = connection;
            this.data = data;

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
            RequestInClient request = new RequestInClient(data, this);
            ResponseInClient response = new ResponseInClient(request, this);
            response.Post();
        }

        void HandleDataInServer()
        {
            RequestInServer request = new RequestInServer(data, this);
            ResponseInServer response = new ResponseInServer(request, this);
            response.Post(response.sendToAllClients);
        }
    }
}