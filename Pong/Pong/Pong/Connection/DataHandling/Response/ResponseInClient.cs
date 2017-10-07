using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    public class ResponseInClient : Response
    {
        public ResponseInClient(RequestInClient request, DataHandler dataHandler)
        {
            if (request.ResponseExpected)
            {
                this.request = request;
                this.dataHandler = dataHandler;
                ProcessData(request);
            }
        }

        void ProcessData(RequestInClient request)
        {
            switch (request.Type)
            {
            }
        }
    }
}
