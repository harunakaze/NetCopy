using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCopy {
    class WindowHandler {

        private IKeyboardMouseEvents m_GlobalHook;

        private bool sendingPaste = false;
        private bool winKeyIsPressed = false;

        public WindowHandler() {
            Subscribe();
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

            if (e.KeyCode == Keys.C && e.Control) {
                //TODO: SEND DATA TO NETWORK HERE
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

        public void PasteData() {
            //Backup clipboard
            ClipboardData clipboardData = new ClipboardData();
            clipboardData.BackupData();
            
            //TODO: GET DATA FROM SERVER
            Clipboard.SetText("GET FROM SERVER");

            //TODO: Preferably use other method to do this
            //Send CTRL + V
            SendKeys.SendWait("^{V}");

            //Restore clipboard
            clipboardData.RestoreData();
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
