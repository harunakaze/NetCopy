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
        private WindowHandler windowHandler;

        public MainWindow() {
            InitializeComponent();
            windowHandler = new WindowHandler();
            FormClosing += FormClosingHandler;
        }

        private void FormClosingHandler(object sender, FormClosingEventArgs e) {
            if (windowHandler != null) {
                windowHandler.Unsubscribe();
            }
        }

    }
}
