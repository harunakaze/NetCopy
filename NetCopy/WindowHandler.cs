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
        [DllImport("USER32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("USER32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public struct RECT {
            public uint left;
            public uint top;
            public uint right;
            public uint bottom;
        }

        public struct LPGUITHREADINFO {
            public uint cbSize;
            public uint flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCapred;
            public RECT rcCaret;
        };

        [DllImport("user32.dll", EntryPoint = "GetGUIThreadInfo")]
        public static extern bool GetGUIThreadInfo(uint idThread, ref LPGUITHREADINFO threadInfo);

        public void PasteData() {
            //Get foreground apps
            IntPtr foregroundWindow = GetForegroundWindow();
            //IntPtr foregroundWindow = GetFocusedHandle();
            //SetForegroundWindow(foregroundWindow);

            //Send CTRL + V
            SendKeys.Send("^{V}");
            //SendKeys.SendWait("Y");
            const uint WM_COMMAND = 0x0111;
            const uint WM_PASTE = 0x0302;
            //SendMessage(foregroundWindow, WM_PASTE, IntPtr.Zero, IntPtr.Zero);

            Console.WriteLine("Called");
            Console.WriteLine("Activehandle " + GetActiveWindowTitle(GetFocusedHandle()));
        }

        static IntPtr GetFocusedHandle() {
            LPGUITHREADINFO info = new LPGUITHREADINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            if (!GetGUIThreadInfo((uint)0, ref info))
                Console.WriteLine("ERROR! GETTING WINDOWS HANDLE!");
            return info.hwndFocus;
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
