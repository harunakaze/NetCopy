using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCopy {
    class NetCopy {

        private ServerData serverData;

        private ClipboardData clipboardData;

        private bool isPasting = false;

        public NetCopy() {
            clipboardData = new ClipboardData();
            clipboardData.SetClipboardText();
            SendData();

            serverData = new ServerData();
        }

        public void PasteData() {
            try {
                isPasting = true;

                //Backup clipboard
                clipboardData.BackupData();

                //TODO: GET DATA FROM SERVER
                string serverText = serverData.RequestServerData();

                Clipboard.SetText(serverText);

                //TODO: Preferably use other method to do this
                //Send CTRL + V
                SendKeys.SendWait("^{V}");

                //Restore clipboard
                clipboardData.RestoreData();

                isPasting = false;
            }
            catch (Exception e) {
                Console.WriteLine("Paste Data Error : " + e);
            }
        }

        public void SendData() {
            if (!Clipboard.ContainsText())
                return;

            if (isPasting)
                return;

            Console.WriteLine("    \nSENDING DATA...");
            NetworkClient netClient = new NetworkClient();

            clipboardData.SetClipboardText();
            string data = clipboardData.GetSendedText();
            string responseMessage = null;

            Thread clientThread = new Thread(() => netClient.StartClient(data, out responseMessage));
            clientThread.Start();
        }
    }
}
