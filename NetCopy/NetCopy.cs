using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetLibrary;
using System.Net;

namespace NetCopy {
    class NetCopy {
        private ServerRequest serverData;
        private ClipboardData clipboardData;

        private IPAddress ipAddress;
        private bool isPasting = false;

        public NetCopy(IPAddress ipAddress) {
            this.ipAddress = ipAddress;

            clipboardData = new ClipboardData();
            clipboardData.SetClipboardText();
            SendData();

            serverData = new ServerRequest(this.ipAddress);
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
            catch (ServerOfflineException error) {
                MessageBox.Show(error.Message, "Server is offline", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            Thread clientThread = new Thread(() => netClient.StartClient(ipAddress, data, out responseMessage));
            clientThread.Start();
        }
    }
}
