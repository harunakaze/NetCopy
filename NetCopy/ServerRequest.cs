using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCopy {
    class ServerRequest {
        private const string REQUEST_QUERY = "!@#REQUEST_DATA#@!<!@#!@#)?>";
        private const string CHECK_QUERY = "!@#CHECK_DATA#@!<!@#!@#)?>";
        private const int REQUST_TIMEOUT_MS = 200;
        private IPAddress serverIP;

        public ServerRequest(IPAddress serverIP) {
            this.serverIP = serverIP;
            RequestServerData();
        }

        public string RequestServerData() {
            return MakeRequest(REQUEST_QUERY);
        }

        private string MakeRequest(string request) {
            Console.WriteLine("    \nCHECKING SERVER...");

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
