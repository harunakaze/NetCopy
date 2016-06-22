using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetCopy {
    public partial class MainWindow : MaterialForm {
        private NetCopy netCopy;
        private UserInputHandler windowHandler;

        public MainWindow() {
            Console.WriteLine("\n================================= NEW PROCESS =================================\n");
            InitializeComponent();

            netCopy = new NetCopy();
            windowHandler = new UserInputHandler(netCopy);

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
