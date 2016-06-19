using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCopy {
    public partial class MainWindow : Form {

        private IKeyboardMouseEvents m_GlobalHook;
        private String ClipboardText = null;
        private WindowHandler windowHandler;
        private bool sendingPaste = false;
        private bool winIsPressed = false;

        public MainWindow() {
            InitializeComponent();

            Subscribe();
            FormClosing += Form_Closing;

            windowHandler = new WindowHandler();
            ClipboardText = Clipboard.GetText();
        }


        private void Form_Closing(object sender, FormClosingEventArgs e) {
            Unsubscribe();
        }


        public void Subscribe() {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
            m_GlobalHook.KeyDown += OnKeyDown;
            m_GlobalHook.KeyUp += OnKeyUp;
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            Console.WriteLine("OnKeyDown: \t{0}", e.KeyCode);

            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) {
                winIsPressed = true;
            }

            if (e.KeyCode == Keys.C && e.Control) {
                //Console.WriteLine(Clipboard.GetText());
                ClipboardText = Clipboard.GetText();
            }

            //if (e.KeyCode == Keys.V && e.Control && e.Alt && !sendingPaste) {
            //if (e.KeyCode == Keys.H && !sendingPaste) {
            //if (e.KeyCode == Keys.F12 && e.Control && !sendingPaste) {
            if (e.KeyCode == Keys.V && winIsPressed && !sendingPaste) {
                sendingPaste = true;

                Console.WriteLine("CTRL + ALT + V");
                if (ClipboardText != null)
                    Console.WriteLine("THE TEXT ARE : " + ClipboardText);

                windowHandler.PasteData();
                //SendKeys.Send("k");
                //InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
            }

        }

        private void OnKeyUp(Object sender, KeyEventArgs e) {
            //Console.WriteLine("OnKeyUp: \t{0}", e.KeyCode);
            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) {
                winIsPressed = false;
            }
            if (e.KeyCode == Keys.V) {
            //if (e.KeyCode == Keys.H) {
            //if (e.KeyCode == Keys.F12) {
                sendingPaste = false;
                Console.WriteLine("Pulled out");
            }
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e) {
            //Console.WriteLine("KeyPress: \t{0}", e.KeyChar);
        }

        public void Unsubscribe() {
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;
            m_GlobalHook.KeyDown -= OnKeyDown;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }
    }
}
