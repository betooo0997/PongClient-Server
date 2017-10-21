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
        /// <summary>
        /// The incoming data.
        /// </summary>
        string data;

        /// <summary>
        /// The ConnectionHandler instance that created this DataHandler instance.
        /// </summary>
        public ConnectionHandler connection { get; private set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="connection">The ConnectionHandler instance that created this DataHandler instance.</param>
        /// <param name="data">The incoming data</param>
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

        /// <summary>
        /// Method for the clietns instances.
        /// Creates a Request instance to analyze the incoming data, a Response instance to create a valid answer to it and send it to the socket if required.
        /// </summary>
        void HandleDataInClient()
        {
            RequestInClient request = new RequestInClient(data, this);
            ResponseInClient response = new ResponseInClient(request, this);
            response.Post();
        }

        /// <summary>
        /// Method for the server instance.
        /// Creates a Request instance to analyze the incoming data, a Response instance to create a valid answer to it and send it to the socket if required.
        /// </summary>
        void HandleDataInServer()
        {
            RequestInServer request = new RequestInServer(data, this);
            ResponseInServer response = new ResponseInServer(request, this);
            response.Post(response.sendToAllClients);
        }
    }
}