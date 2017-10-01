using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace PongServer
{
    class HandleClient
    {
        Socket handler;

        public HandleClient(Socket handler)
        {
            this.handler = handler;

            HandleClientData();
        }

        void HandleClientData()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];

                int a = handler.Receive(bytes);

                Request request = new Request(handler, bytes, a);
            }
        }
    }
}
