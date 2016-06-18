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
    public partial class Form1 : Form {

        private IKeyboardMouseEvents m_GlobalHook;
        private  String ClipboardText = null;

        public Form1() {
            InitializeComponent();

            Subscribe();
            FormClosing += Form_Closing;
        }


        private void Form_Closing(object sender, FormClosingEventArgs e) {
            Unsubscribe();
        }


        public void Subscribe() {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
            m_GlobalHook.KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            //Console.WriteLine("OnKeyDown: \t{0}", e.KeyCode);

            if (e.KeyCode == Keys.C && e.Control) {
                //Console.WriteLine(Clipboard.GetText());
                ClipboardText = Clipboard.GetText();
            }

            if (e.KeyCode == Keys.V && e.Control && e.Alt) {
                if (ClipboardText != null)
                    Console.WriteLine("THE TEXT ARE : " + ClipboardText);
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
