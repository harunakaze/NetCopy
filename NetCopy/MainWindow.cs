using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetCopy {
    public partial class MainWindow : Form {
        private WindowHandler windowHandler;

        public MainWindow() {
            InitializeComponent();
            windowHandler = new WindowHandler();
            FormClosing += FormClosingHandler;
        }

        private void FormClosingHandler(object sender, FormClosingEventArgs e) {
            Debug.Assert(windowHandler != null, "WINDOW HANDLER NOT INITIALIZED!", "Please check window handler class!");
            if (windowHandler != null) {
                windowHandler.Unsubscribe();
            }
        }

    }
}
