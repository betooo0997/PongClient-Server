using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Pong
{
    public abstract class Request
    {
        protected byte[] bytes;
        protected int a;

        public bool ResponseExpected;
    }
}
