using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NetCopy {
    class WindowHandler : Form {

        private IKeyboardMouseEvents m_GlobalHook;

        private bool sendingPaste = false;
        private bool winKeyIsPressed = false;

        ClipboardData clipboardData;

        //Clipboard listener
        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private IntPtr nextClipboardViewer;
        

        public WindowHandler() {
            Subscribe();
            clipboardData = new ClipboardData();
            clipboardData.SetClipboardText();
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);
        }

        private void Subscribe() {
            //Register Global key hook
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyDown += OnKeyDown;
            m_GlobalHook.KeyUp += OnKeyUp;
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) {
                winKeyIsPressed = true;
            }

            if (e.KeyCode == Keys.V && winKeyIsPressed && !sendingPaste) {
                //TODO : REQUEST DATA FROM NETWORK HERE?
                sendingPaste = true;
                PasteData();
            }

        }

        private void OnKeyUp(Object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) {
                winKeyIsPressed = false;
            }

            if (e.KeyCode == Keys.V) {
                sendingPaste = false;
            }
        }

        public void Unsubscribe() {
            //Removing listener function
            m_GlobalHook.KeyDown -= OnKeyDown;
            m_GlobalHook.KeyUp -= OnKeyUp;

            //Fisposing global hook
            m_GlobalHook.Dispose();
        }

        private void PasteData() {
            //Backup clipboard
            clipboardData.BackupData();
            
            //TODO: GET DATA FROM SERVER
            Clipboard.SetText("GET FROM SERVER");

            //TODO: Preferably use other method to do this
            //Send CTRL + V
            SendKeys.SendWait("^{V}");

            //Restore clipboard
            clipboardData.RestoreData();
        }

        private void SendData() {
            //Task.Factory.StartNew(() => NetworkClient.StartClient());            
            //NetworkClient.StartClient();
            //Task clientTask = new Task(() => NetworkClient.StartClient());
            //clientTask.Start();

            NetworkClient netClient = new NetworkClient();

            clipboardData.SetClipboardText();
            string data = clipboardData.GetSendedText();

            Thread clientThread = new Thread(() => netClient.StartClient(data));
            clientThread.SetApartmentState(ApartmentState.STA);
            clientThread.Start();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m) {
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg) {
                case WM_DRAWCLIPBOARD:
                    
                    SendData();

                    SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                        nextClipboardViewer = m.LParam;
                    else
                        SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void Dispose(bool disposing) {
            ChangeClipboardChain(this.Handle, nextClipboardViewer);
            base.Dispose(disposing);
        }

        #region Debug code
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle(IntPtr handle) {
            const int nChars = 256;
            StringBuilder buffer = new StringBuilder(nChars);

            if (GetWindowText(handle, buffer, nChars) > 0)
            {
                Console.WriteLine(buffer.ToString());
                return buffer.ToString();
            }
            return null;
        }

        #endregion

    }
}
