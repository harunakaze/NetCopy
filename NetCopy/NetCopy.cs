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

        private MainWindow mainWindow;

        public NetCopy(IPAddress ipAddress, MainWindow mainWindow) {
            this.ipAddress = ipAddress;
            this.mainWindow = mainWindow;

            clipboardData = new ClipboardData();
            clipboardData.SetClipboardText();
            //SendData();

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
                //NOTE: This may cause key up, or down call related bug
                SendKeys.SendWait("^{V}");

                //Restore clipboard
                clipboardData.RestoreData();

                isPasting = false;
            }
            catch (ServerOfflineException error) {
                MessageBox.Show(error.Message, "Server is offline", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mainWindow.ResetForm();
            }
            catch (Exception e) {
                Console.WriteLine("Paste Data Error : " + e);
            }
        }

        //Send clipboard text to server
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
