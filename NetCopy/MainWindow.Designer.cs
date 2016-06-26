namespace NetCopy {
    partial class MainWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.textBoxIP = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.buttonConnect = new MaterialSkin.Controls.MaterialFlatButton();
            this.SuspendLayout();
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.BackColor = System.Drawing.Color.Transparent;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(13, 74);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(68, 19);
            this.materialLabel1.TabIndex = 0;
            this.materialLabel1.Text = "Server IP";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Depth = 0;
            this.textBoxIP.Hint = "";
            this.textBoxIP.Location = new System.Drawing.Point(17, 97);
            this.textBoxIP.MouseState = MaterialSkin.MouseState.HOVER;
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.PasswordChar = '\0';
            this.textBoxIP.SelectedText = "";
            this.textBoxIP.SelectionLength = 0;
            this.textBoxIP.SelectionStart = 0;
            this.textBoxIP.Size = new System.Drawing.Size(255, 23);
            this.textBoxIP.TabIndex = 1;
            this.textBoxIP.UseSystemPasswordChar = false;
            this.textBoxIP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxIP_KeyDown);
            // 
            // buttonConnect
            // 
            this.buttonConnect.AutoSize = true;
            this.buttonConnect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonConnect.Depth = 0;
            this.buttonConnect.Location = new System.Drawing.Point(17, 129);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonConnect.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Primary = false;
            this.buttonConnect.Size = new System.Drawing.Size(75, 36);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.materialLabel1);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Net Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialSingleLineTextField textBoxIP;
        private MaterialSkin.Controls.MaterialFlatButton buttonConnect;


    }
}

