﻿using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace NetCopy {
    public partial class MainWindow : MaterialForm {
        private NetCopy netCopy;
        private UserInputHandler windowHandler;

        public MainWindow() {
            Console.WriteLine("\n================================= NEW PROCESS =================================\n");
            InitializeComponent();
        }

        private void FormClosingHandler(object sender, FormClosingEventArgs e) {
            Debug.Assert(windowHandler != null, "WINDOW HANDLER NOT INITIALIZED!", "Please check window handler class!");
            if (windowHandler != null) {
                windowHandler.Unsubscribe();
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e) {
            try {
                TryConnect();

                //UI Handling
                textBoxIP.Enabled = false;

                buttonConnect.Text = "Connected!";
                buttonConnect.Enabled = false;
            }
            catch (FormatException error) {
                MessageBox.Show(error.Message, "Invalid Server IP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ServerOfflineException error) {
                MessageBox.Show(error.Message, "Server is offline", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception error) {
                MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxIP_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                buttonConnect_Click(this, new EventArgs());
            }
        }

        private void TryConnect() {
            string ipAddressString = textBoxIP.Text;
            IPAddress ipAddress = IPAddress.Parse(ipAddressString);

            netCopy = new NetCopy(ipAddress);

            windowHandler = new UserInputHandler(netCopy);
            FormClosing += FormClosingHandler;
        }

    }
}
