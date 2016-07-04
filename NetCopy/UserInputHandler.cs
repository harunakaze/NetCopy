using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NetCopy {
    class UserInputHandler : NativeWindow {

        private IKeyboardMouseEvents m_GlobalHook;

        private bool sendingPaste = false;
        private bool winKeyIsPressed = false;

        private NetCopy netCopy;

        //Clipboard listener
        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private IntPtr nextClipboardViewer;
        

        public UserInputHandler(NetCopy instance) {
            this.CreateHandle(new CreateParams());

            netCopy = instance;

            Subscribe();

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
                netCopy.PasteData();
            }

        }

        private void OnKeyUp(Object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) {
                winKeyIsPressed = false;
            }

            //NOTE: The CTRL + V in NetCopy.PasteData() cause a bug here
            //That function call OnKeyUp() unintentionally, fix that
            if (e.KeyCode == Keys.V) {
                sendingPaste = false;
            }
        }

        public void Unsubscribe() {
            //Removing listener function
            m_GlobalHook.KeyDown -= OnKeyDown;
            m_GlobalHook.KeyUp -= OnKeyUp;

            //Disposing global hook
            m_GlobalHook.Dispose();

            //Release WndProc listener
            ReleaseHandle();
        }

        protected override void WndProc(ref Message m) {
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg) {
                case WM_DRAWCLIPBOARD:

                    netCopy.SendData();

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
