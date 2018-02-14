using System;
using System.Drawing;
using System.Windows.Forms;
using WobbrockLib.Net;

namespace TcpSendText
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		#region Fields

		NetSender _sender;

		#endregion

		#region Form Elements

		private System.Windows.Forms.TextBox txtToSend;
		private System.Windows.Forms.TextBox txtIP;
		private System.Windows.Forms.NumericUpDown numPort;
		private System.Windows.Forms.Label lblIP;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.Button cmdConnect;
		private System.Windows.Forms.Label lblConnected;
        private Button cmdClear;
        private System.ComponentModel.Container components = null;

		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		public MainForm()
		{
			InitializeComponent();
			_sender = new NetSender();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtToSend = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.lblConnected = new System.Windows.Forms.Label();
            this.cmdClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // txtToSend
            // 
            this.txtToSend.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToSend.Location = new System.Drawing.Point(8, 8);
            this.txtToSend.Name = "txtToSend";
            this.txtToSend.Size = new System.Drawing.Size(616, 38);
            this.txtToSend.TabIndex = 0;
            this.txtToSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtToSend_KeyDown);
            this.txtToSend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtToSend_KeyPress);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(72, 48);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(112, 20);
            this.txtIP.TabIndex = 3;
            this.txtIP.Text = "127.0.0.1";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(224, 48);
            this.numPort.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(64, 20);
            this.numPort.TabIndex = 5;
            this.numPort.Value = new decimal(new int[] {
            9080,
            0,
            0,
            0});
            // 
            // lblIP
            // 
            this.lblIP.Location = new System.Drawing.Point(8, 52);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(64, 16);
            this.lblIP.TabIndex = 2;
            this.lblIP.Text = "IP Address";
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(192, 52);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(32, 16);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port";
            // 
            // cmdConnect
            // 
            this.cmdConnect.Location = new System.Drawing.Point(296, 48);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(75, 20);
            this.cmdConnect.TabIndex = 6;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // lblConnected
            // 
            this.lblConnected.ForeColor = System.Drawing.Color.Red;
            this.lblConnected.Location = new System.Drawing.Point(544, 52);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(80, 16);
            this.lblConnected.TabIndex = 7;
            this.lblConnected.Text = "Not Connected";
            // 
            // cmdClear
            // 
            this.cmdClear.FlatAppearance.BorderSize = 0;
            this.cmdClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdClear.ForeColor = System.Drawing.Color.DarkGray;
            this.cmdClear.Location = new System.Drawing.Point(603, 17);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(16, 20);
            this.cmdClear.TabIndex = 1;
            this.cmdClear.Text = "X";
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 70);
            this.Controls.Add(this.lblConnected);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.cmdConnect);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.txtToSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Send Text";
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        private void cmdConnect_Click(object sender, System.EventArgs e)
        {
            _sender.IP = txtIP.Text;
            _sender.Port = (int) numPort.Value;
            if (_sender.Connect())
            {
                lblConnected.Text = "Connected";
                lblConnected.ForeColor = Color.Blue;
            }
            else
            {
                lblConnected.Text = "Not Connected";
                lblConnected.ForeColor = Color.Red;
            }
        }

        private void txtToSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (_sender.IsConnected)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    char esc = (char) 27;
                    bool sent = _sender.Send(esc.ToString()); // Esc
                    if (!sent)
                    {
                        lblConnected.Text = "Not Connected";
                        lblConnected.ForeColor = Color.Red;
                    }
                    e.Handled = true;
                }
            }
        }

        private void txtToSend_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (_sender.IsConnected)
            {
                bool sent = _sender.Send(e.KeyChar.ToString());
                if (!sent)
                {
                    lblConnected.Text = "Not Connected";
                    lblConnected.ForeColor = Color.Red;
                }
                if (e.KeyChar == 13) // Enter
                {
                    txtToSend.Clear();
                    e.Handled = true;
                }
            }
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            txtToSend.Clear();
        }

        
    }
}
