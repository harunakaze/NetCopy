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
        private string pasteData;
        private IPAddress serverIP;

        public ServerRequest(IPAddress serverIP) {
            this.serverIP = serverIP;
            ServerIsAvailable();
            RequestServerData();
        }

        private void ServerIsAvailable() {
            Console.WriteLine("    \nCHECKING SERVER...");

            NetworkClient netClient = new NetworkClient();

            string serverResponse = null;

            Thread clientThread = new Thread(() => netClient.StartClient(serverIP, CHECK_QUERY, out serverResponse));
            clientThread.Start();

            //TODO: Find other method of waiting this process, so it make a responsive impression?
            clientThread.Join(REQUST_TIMEOUT_MS);

            if (serverResponse == null) {
                throw new ServerOfflineException();
            }
        }

        public string RequestServerData() {
            Console.WriteLine("    \nREQUESTING DATA...");

            NetworkClient netClient = new NetworkClient();

            Thread clientThread = new Thread(() => netClient.StartClient(serverIP, REQUEST_QUERY, out pasteData));
            clientThread.Start();

            //TODO: Find other method of waiting this process, so it make a responsive impression?
            clientThread.Join(REQUST_TIMEOUT_MS);

            Console.WriteLine("DONE REQUESTING DATA, DATA : " + pasteData);

            return pasteData;
        }
    }
}
