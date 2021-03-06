﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCopy {
    class ServerRequest {
        private const string REQUEST_FOR_DATA_QUERY = "!@#REQUEST_DATA#@!<!@#!@#)?>";
        private const string CHECK_SERVER_QUERY = "!@#CHECK_DATA#@!<!@#!@#)?>";
        private const int REQUST_TIMEOUT_MS = 200;
        private IPAddress serverIP;

        public ServerRequest(IPAddress serverIP) {
            this.serverIP = serverIP;
            //RequestServerData();
            //Check server is online
            MakeRequest(CHECK_SERVER_QUERY);
        }

        public string RequestServerData() {
            return MakeRequest(REQUEST_FOR_DATA_QUERY);
        }

        private string MakeRequest(string request) {
            Console.WriteLine("    \nREQUESTING FROM SERVER... REQUEST QUERY : {0}", request);

            NetworkClient netClient = new NetworkClient();

            string serverResponse = null;

            Thread clientThread = new Thread(() => netClient.StartClient(serverIP, request, out serverResponse));
            clientThread.Start();

            //TODO: Find other method of waiting this process, so it make a responsive impression?
            clientThread.Join(REQUST_TIMEOUT_MS);

            if (serverResponse == null) {
                throw new ServerOfflineException();
            }

            return serverResponse;
        }
    }
}
