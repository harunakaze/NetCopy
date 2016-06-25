using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetPaste {
    public partial class MainWindow : Form {
        public MainWindow() {
            InitializeComponent();

            //Task.Factory.StartNew(() => NetworkServer.StartListening());
            //NetworkServer.StartListening();

            Task serverTask = new Task(() => NetworkServer.StartListening());
            serverTask.Start();
        }
    }
}
