using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Pong
{
    public abstract class Response
    {
        /// <summary>
        /// The Responses request instance.
        /// </summary>
        protected Request request;

        /// <summary>
        /// The bytedata to be sent to the client-
        /// </summary>
        protected byte[] bytedata;
        protected DataHandler dataHandler;

        /// <summary>
        /// Sends the data to the client/s.
        /// </summary>
        /// <param name="sendToAllClients">If the data should be sent to all clients.</param>
        public void Post(bool sendToAllClients = false)
        {
            try
            {
                if (bytedata != null)
                {
                    if (!sendToAllClients)
                    {
                        dataHandler.connection.socket.Send(bytedata);
                    }
                    else
                    {
                        foreach (ConnectionHandler connection in ConnectionHandler.connections)
                            connection.socket.Send(bytedata);
                    }
                    Console.WriteLine("DATA SENT: " + Encoding.UTF8.GetString(bytedata));
                }
            }
            catch
            {
                ConnectionHandler.Error();
            }
        }
    }
}
