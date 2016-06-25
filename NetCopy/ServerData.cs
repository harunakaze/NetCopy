using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetCopy {
    class ServerData {
        private const string REQUEST_QUERY = "!@#REQUEST_DATA#@!<!@#!@#)?>";
        private const int REQUST_TIMEOUT_MS = 1500;
        private string pasteData;

        public ServerData() {
            RequestServerData();
        }

        public string RequestServerData() {
            Console.WriteLine("    \nREQUESTING DATA...");

            NetworkClient netClient = new NetworkClient();

            Thread clientThread = new Thread(() => netClient.StartClient(REQUEST_QUERY, out pasteData));
            clientThread.Start();

            //TODO: Find other method of waiting this process, so it make a responsive impression?
            clientThread.Join(REQUST_TIMEOUT_MS);

            Console.WriteLine("DONE REQUESTING DATA, DATA : " + pasteData);

            return pasteData;
        }
    }
}
