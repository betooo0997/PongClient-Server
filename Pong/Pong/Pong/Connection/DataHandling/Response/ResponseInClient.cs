using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    /// <summary>
    /// Class that manages the Responses inside a client.
    /// </summary>
    public class ResponseInClient : Response
    {
        /// <summary>
        /// The class constructor.
        /// </summary>
        /// <param name="request">The analyzed data.</param>
        /// <param name="dataHandler">The DataHandler instance that created this instance.</param>
        public ResponseInClient(RequestInClient request, DataHandler dataHandler)
        {
            if (request.ResponseExpected)
            {
                this.request = request;
                this.dataHandler = dataHandler;
                ProcessData(request);
            }
        }

        /// <summary>
        /// Creates a an answer based on the incoming request data.
        /// </summary>
        /// <param name="request"></param>
        void ProcessData(RequestInClient request)
        {
            switch (request.Type)
            {
            }
        }
    }
}
