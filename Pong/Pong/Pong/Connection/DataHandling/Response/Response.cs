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
        protected Request request;
        protected byte[] bytedata;
        protected DataHandler dataHandler;

        public void Post()
        {
            if (bytedata != null)
            {
                dataHandler.connection.socket.Send(bytedata);
                Console.WriteLine("DATA SENT: " + Encoding.UTF8.GetString(bytedata));
            }
        }
    }
}
