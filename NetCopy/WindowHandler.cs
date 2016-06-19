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

        private String ClipboardText = null;
        private bool sendingPaste = false;
        private bool winKeyIsPressed = false;

        public WindowHandler() {
            ClipboardText = Clipboard.GetText();
            Subscribe();
        }

        public void Subscribe() {
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
                ClipboardText = Clipboard.GetText();
            }

            if (e.KeyCode == Keys.V && winKeyIsPressed && !sendingPaste) {
                sendingPaste = true;

                if (ClipboardText != null) {
                    PasteData();
                }
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
            //Send CTRL + V
            SendKeys.Send("^{V}");
        }

        #region Debug code
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle(IntPtr handle) {
            const int nChars = 256;
            StringBuilder buffer = new StringBuilder(nChars);
            //IntPtr handle = GetForegroundWindow();

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
