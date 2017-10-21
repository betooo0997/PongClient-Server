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
        /// <summary>
        /// If the request expects to be answered or not.
        /// </summary>
        public bool ResponseExpected;
    }
}
