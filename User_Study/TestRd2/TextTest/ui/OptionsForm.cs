using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace TextTest
{
	public class OptionsForm : System.Windows.Forms.Form
	{
		#region Options inner class

		public class Options
		{
			public bool NoPunctuation;	// disallow punctuation
			public bool NoNumbers;		// disallow numeric digits
			public bool NoCapitals;		// present and allow lowercase only
			
			public bool NoBackspace;	// disallow backspaces
			public bool	NoWordBackspace;// disallow word backspaces
			public bool NoEnter;		// disallow enter (advances the task)

			public bool AutoStop;		// automatically stop the test
			public int  StopAfter;		// stop the test after this task
			public bool AutoQuit;		// true = close the app; false = just stop the test

			public bool AutoSwitch;		// automatically switch from practice to test
			public int  SwitchAfter;	// switch after this task number

			public bool SendOnTcp;		// send characters inputted on a Tcp port
			public int  SendPort;		// port number to send on
			public string SendIP;		// IP address to send to

			public bool ReceiveOnNet;	// receive characters on the network
            public bool UseWebSockets;  // receive via the WebSockets protocol (or TCP if false)
			public int  ReceivePort;	// port number to receive on
			public string ReceiveIP;	// IP address to receive from

            public bool Echo;           // echo characters back to client
            public bool SendPres;       // send presented phrases to client

            // defaults
            public Options()
            {
                NoPunctuation = false;
                NoNumbers = false;
                NoCapitals = false;

                NoBackspace = false;
                NoWordBackspace = false;
                NoEnter = false;

                AutoStop = true;
                StopAfter = 18;
                AutoQuit = false;

                AutoSwitch = true;
                SwitchAfter = 3;

                ReceiveOnNet = false;
                UseWebSockets = false;
                ReceivePort = 9080;
                ReceiveIP = "127.0.0.1";

                Echo = false;
                SendPres = false;

                SendOnTcp = false;
                SendPort = 9079;
                SendIP = "127.0.0.1";
            }

            // copy constructor
            public Options(OptionsForm.Options toCopy)
			{
				NoPunctuation = toCopy.NoPunctuation;
				NoNumbers = toCopy.NoNumbers;
				NoCapitals = toCopy.NoCapitals;
				
				NoEnter = toCopy.NoEnter;
				NoBackspace = toCopy.NoBackspace;
				NoWordBackspace = toCopy.NoWordBackspace;
				
				AutoStop = toCopy.AutoStop;
				StopAfter = toCopy.StopAfter;
				AutoQuit = toCopy.AutoQuit;
				
				AutoSwitch = toCopy.AutoSwitch;
				SwitchAfter = toCopy.SwitchAfter;
				
				ReceiveOnNet = toCopy.ReceiveOnNet;
                UseWebSockets = toCopy.UseWebSockets;
				ReceivePort = toCopy.ReceivePort;
				ReceiveIP = toCopy.ReceiveIP;

                Echo = toCopy.Echo;
                SendPres = toCopy.SendPres;

                SendOnTcp = toCopy.SendOnTcp;
                SendPort = toCopy.SendPort;
                SendIP = toCopy.SendIP;
            }

            public Options Copy()
			{
				return new Options(this);
			}
		}

		#endregion

		private OptionsForm.Options _o;

		#region Form Elements

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TabControl tabOptions;
		private System.Windows.Forms.TabPage tpgAutomatic;
		private System.Windows.Forms.TabPage tpgPrevent;
		private System.Windows.Forms.TabPage tpgNetwork;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.NumericUpDown numSwitch;
		private System.Windows.Forms.CheckBox chkSwitch;
		private System.Windows.Forms.RadioButton optExit;
		private System.Windows.Forms.RadioButton optStop;
		private System.Windows.Forms.NumericUpDown numStop;
		private System.Windows.Forms.CheckBox chkStop;
		private System.Windows.Forms.GroupBox grpAutomatic;
		private System.Windows.Forms.GroupBox grpPrevent;
		private System.Windows.Forms.CheckBox chkNoBksp;
		private System.Windows.Forms.CheckBox chkNoPunct;
		private System.Windows.Forms.CheckBox chkNoNums;
		private System.Windows.Forms.CheckBox chkNoCaps;
		private System.Windows.Forms.GroupBox grpServer;
		private System.Windows.Forms.CheckBox chkSend;
		private System.Windows.Forms.CheckBox chkReceive;
		private System.Windows.Forms.NumericUpDown numSendPort;
		private System.Windows.Forms.TextBox txtSendIP;
		private System.Windows.Forms.TextBox txtRecIP;
		private System.Windows.Forms.Label lblSendIP;
		private System.Windows.Forms.Label lblRecIP;
		private System.Windows.Forms.CheckBox chkNoEnter;
		private System.Windows.Forms.CheckBox chkNoWordBksp;
        private ComboBox cboRecProtocol;
        private GroupBox grpClient;
        private Label lblSendPort;
        private CheckBox chkSendPres;
        private CheckBox chkEcho;
        private Label lblRecPort;
        private ComboBox cboSendProtocol;
        private System.Windows.Forms.NumericUpDown numRecPort;

		#endregion

		public OptionsForm()
		{
			InitializeComponent();
			_o = new Options(); // defaults
		}

		public OptionsForm(Options o)
		{
			InitializeComponent();
			_o = new Options(o); // copy ctor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tpgAutomatic = new System.Windows.Forms.TabPage();
            this.grpAutomatic = new System.Windows.Forms.GroupBox();
            this.numSwitch = new System.Windows.Forms.NumericUpDown();
            this.optExit = new System.Windows.Forms.RadioButton();
            this.optStop = new System.Windows.Forms.RadioButton();
            this.numStop = new System.Windows.Forms.NumericUpDown();
            this.chkSwitch = new System.Windows.Forms.CheckBox();
            this.chkStop = new System.Windows.Forms.CheckBox();
            this.tpgPrevent = new System.Windows.Forms.TabPage();
            this.grpPrevent = new System.Windows.Forms.GroupBox();
            this.chkNoWordBksp = new System.Windows.Forms.CheckBox();
            this.chkNoEnter = new System.Windows.Forms.CheckBox();
            this.chkNoCaps = new System.Windows.Forms.CheckBox();
            this.chkNoNums = new System.Windows.Forms.CheckBox();
            this.chkNoPunct = new System.Windows.Forms.CheckBox();
            this.chkNoBksp = new System.Windows.Forms.CheckBox();
            this.tpgNetwork = new System.Windows.Forms.TabPage();
            this.grpClient = new System.Windows.Forms.GroupBox();
            this.cboSendProtocol = new System.Windows.Forms.ComboBox();
            this.chkSend = new System.Windows.Forms.CheckBox();
            this.lblSendIP = new System.Windows.Forms.Label();
            this.lblSendPort = new System.Windows.Forms.Label();
            this.txtSendIP = new System.Windows.Forms.TextBox();
            this.numSendPort = new System.Windows.Forms.NumericUpDown();
            this.grpServer = new System.Windows.Forms.GroupBox();
            this.chkSendPres = new System.Windows.Forms.CheckBox();
            this.chkEcho = new System.Windows.Forms.CheckBox();
            this.lblRecPort = new System.Windows.Forms.Label();
            this.cboRecProtocol = new System.Windows.Forms.ComboBox();
            this.lblRecIP = new System.Windows.Forms.Label();
            this.txtRecIP = new System.Windows.Forms.TextBox();
            this.numRecPort = new System.Windows.Forms.NumericUpDown();
            this.chkReceive = new System.Windows.Forms.CheckBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.tabOptions.SuspendLayout();
            this.tpgAutomatic.SuspendLayout();
            this.grpAutomatic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSwitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStop)).BeginInit();
            this.tpgPrevent.SuspendLayout();
            this.grpPrevent.SuspendLayout();
            this.tpgNetwork.SuspendLayout();
            this.grpClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSendPort)).BeginInit();
            this.grpServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.tpgAutomatic);
            this.tabOptions.Controls.Add(this.tpgPrevent);
            this.tabOptions.Controls.Add(this.tpgNetwork);
            this.tabOptions.Location = new System.Drawing.Point(0, 0);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(271, 313);
            this.tabOptions.TabIndex = 0;
            // 
            // tpgAutomatic
            // 
            this.tpgAutomatic.Controls.Add(this.grpAutomatic);
            this.tpgAutomatic.Location = new System.Drawing.Point(4, 22);
            this.tpgAutomatic.Name = "tpgAutomatic";
            this.tpgAutomatic.Size = new System.Drawing.Size(263, 287);
            this.tpgAutomatic.TabIndex = 0;
            this.tpgAutomatic.Text = "Auto";
            // 
            // grpAutomatic
            // 
            this.grpAutomatic.Controls.Add(this.numSwitch);
            this.grpAutomatic.Controls.Add(this.optExit);
            this.grpAutomatic.Controls.Add(this.optStop);
            this.grpAutomatic.Controls.Add(this.numStop);
            this.grpAutomatic.Controls.Add(this.chkSwitch);
            this.grpAutomatic.Controls.Add(this.chkStop);
            this.grpAutomatic.Location = new System.Drawing.Point(8, 8);
            this.grpAutomatic.Name = "grpAutomatic";
            this.grpAutomatic.Size = new System.Drawing.Size(247, 160);
            this.grpAutomatic.TabIndex = 0;
            this.grpAutomatic.TabStop = false;
            this.grpAutomatic.Text = "Automatic Features";
            // 
            // numSwitch
            // 
            this.numSwitch.Enabled = false;
            this.numSwitch.Location = new System.Drawing.Point(184, 116);
            this.numSwitch.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numSwitch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSwitch.Name = "numSwitch";
            this.numSwitch.Size = new System.Drawing.Size(48, 20);
            this.numSwitch.TabIndex = 5;
            this.numSwitch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // optExit
            // 
            this.optExit.Enabled = false;
            this.optExit.Location = new System.Drawing.Point(40, 70);
            this.optExit.Name = "optExit";
            this.optExit.Size = new System.Drawing.Size(120, 16);
            this.optExit.TabIndex = 3;
            this.optExit.Text = "Exit the application";
            // 
            // optStop
            // 
            this.optStop.Checked = true;
            this.optStop.Enabled = false;
            this.optStop.Location = new System.Drawing.Point(40, 50);
            this.optStop.Name = "optStop";
            this.optStop.Size = new System.Drawing.Size(120, 16);
            this.optStop.TabIndex = 2;
            this.optStop.TabStop = true;
            this.optStop.Text = "Stop the test";
            // 
            // numStop
            // 
            this.numStop.Enabled = false;
            this.numStop.Location = new System.Drawing.Point(184, 28);
            this.numStop.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numStop.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStop.Name = "numStop";
            this.numStop.Size = new System.Drawing.Size(48, 20);
            this.numStop.TabIndex = 1;
            this.numStop.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkSwitch
            // 
            this.chkSwitch.Location = new System.Drawing.Point(13, 112);
            this.chkSwitch.Name = "chkSwitch";
            this.chkSwitch.Size = new System.Drawing.Size(177, 24);
            this.chkSwitch.TabIndex = 4;
            this.chkSwitch.Text = "Switch to test mode after trial";
            this.chkSwitch.CheckedChanged += new System.EventHandler(this.chkSwitch_CheckedChanged);
            // 
            // chkStop
            // 
            this.chkStop.Location = new System.Drawing.Point(13, 24);
            this.chkStop.Name = "chkStop";
            this.chkStop.Size = new System.Drawing.Size(144, 24);
            this.chkStop.TabIndex = 0;
            this.chkStop.Text = "Stop the test after trial";
            this.chkStop.CheckedChanged += new System.EventHandler(this.chkStop_CheckedChanged);
            // 
            // tpgPrevent
            // 
            this.tpgPrevent.Controls.Add(this.grpPrevent);
            this.tpgPrevent.Location = new System.Drawing.Point(4, 22);
            this.tpgPrevent.Name = "tpgPrevent";
            this.tpgPrevent.Size = new System.Drawing.Size(263, 287);
            this.tpgPrevent.TabIndex = 1;
            this.tpgPrevent.Text = "Prevention";
            // 
            // grpPrevent
            // 
            this.grpPrevent.Controls.Add(this.chkNoWordBksp);
            this.grpPrevent.Controls.Add(this.chkNoEnter);
            this.grpPrevent.Controls.Add(this.chkNoCaps);
            this.grpPrevent.Controls.Add(this.chkNoNums);
            this.grpPrevent.Controls.Add(this.chkNoPunct);
            this.grpPrevent.Controls.Add(this.chkNoBksp);
            this.grpPrevent.Location = new System.Drawing.Point(8, 8);
            this.grpPrevent.Name = "grpPrevent";
            this.grpPrevent.Size = new System.Drawing.Size(247, 160);
            this.grpPrevent.TabIndex = 0;
            this.grpPrevent.TabStop = false;
            this.grpPrevent.Text = "Prevent Characters";
            // 
            // chkNoWordBksp
            // 
            this.chkNoWordBksp.Location = new System.Drawing.Point(120, 48);
            this.chkNoWordBksp.Name = "chkNoWordBksp";
            this.chkNoWordBksp.Size = new System.Drawing.Size(112, 24);
            this.chkNoWordBksp.TabIndex = 5;
            this.chkNoWordBksp.Text = "Word Backspace";
            // 
            // chkNoEnter
            // 
            this.chkNoEnter.Location = new System.Drawing.Point(13, 112);
            this.chkNoEnter.Name = "chkNoEnter";
            this.chkNoEnter.Size = new System.Drawing.Size(206, 24);
            this.chkNoEnter.TabIndex = 4;
            this.chkNoEnter.Text = "Enter (Default: Advances to next trial)";
            // 
            // chkNoCaps
            // 
            this.chkNoCaps.Location = new System.Drawing.Point(13, 72);
            this.chkNoCaps.Name = "chkNoCaps";
            this.chkNoCaps.Size = new System.Drawing.Size(96, 24);
            this.chkNoCaps.TabIndex = 3;
            this.chkNoCaps.Text = "Capitals";
            // 
            // chkNoNums
            // 
            this.chkNoNums.Location = new System.Drawing.Point(13, 48);
            this.chkNoNums.Name = "chkNoNums";
            this.chkNoNums.Size = new System.Drawing.Size(96, 24);
            this.chkNoNums.TabIndex = 2;
            this.chkNoNums.Text = "Numbers";
            // 
            // chkNoPunct
            // 
            this.chkNoPunct.Location = new System.Drawing.Point(13, 24);
            this.chkNoPunct.Name = "chkNoPunct";
            this.chkNoPunct.Size = new System.Drawing.Size(96, 24);
            this.chkNoPunct.TabIndex = 1;
            this.chkNoPunct.Text = "Punctuation";
            // 
            // chkNoBksp
            // 
            this.chkNoBksp.Location = new System.Drawing.Point(120, 24);
            this.chkNoBksp.Name = "chkNoBksp";
            this.chkNoBksp.Size = new System.Drawing.Size(112, 24);
            this.chkNoBksp.TabIndex = 0;
            this.chkNoBksp.Text = "Backspace";
            this.chkNoBksp.CheckedChanged += new System.EventHandler(this.chkNoBksp_CheckedChanged);
            // 
            // tpgNetwork
            // 
            this.tpgNetwork.Controls.Add(this.grpClient);
            this.tpgNetwork.Controls.Add(this.grpServer);
            this.tpgNetwork.Location = new System.Drawing.Point(4, 22);
            this.tpgNetwork.Name = "tpgNetwork";
            this.tpgNetwork.Size = new System.Drawing.Size(263, 287);
            this.tpgNetwork.TabIndex = 2;
            this.tpgNetwork.Text = "Network";
            // 
            // grpClient
            // 
            this.grpClient.Controls.Add(this.cboSendProtocol);
            this.grpClient.Controls.Add(this.chkSend);
            this.grpClient.Controls.Add(this.lblSendIP);
            this.grpClient.Controls.Add(this.lblSendPort);
            this.grpClient.Controls.Add(this.txtSendIP);
            this.grpClient.Controls.Add(this.numSendPort);
            this.grpClient.Location = new System.Drawing.Point(8, 174);
            this.grpClient.Name = "grpClient";
            this.grpClient.Size = new System.Drawing.Size(247, 106);
            this.grpClient.TabIndex = 1;
            this.grpClient.TabStop = false;
            this.grpClient.Text = "Client";
            // 
            // cboSendProtocol
            // 
            this.cboSendProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSendProtocol.Enabled = false;
            this.cboSendProtocol.FormattingEnabled = true;
            this.cboSendProtocol.Items.AddRange(new object[] {
            "TCP"});
            this.cboSendProtocol.Location = new System.Drawing.Point(141, 21);
            this.cboSendProtocol.Name = "cboSendProtocol";
            this.cboSendProtocol.Size = new System.Drawing.Size(96, 21);
            this.cboSendProtocol.TabIndex = 10;
            // 
            // chkSend
            // 
            this.chkSend.Location = new System.Drawing.Point(6, 19);
            this.chkSend.Name = "chkSend";
            this.chkSend.Size = new System.Drawing.Size(137, 24);
            this.chkSend.TabIndex = 0;
            this.chkSend.Text = "Send characters via";
            this.chkSend.CheckedChanged += new System.EventHandler(this.chkSend_CheckedChanged);
            // 
            // lblSendIP
            // 
            this.lblSendIP.Enabled = false;
            this.lblSendIP.Location = new System.Drawing.Point(71, 77);
            this.lblSendIP.Name = "lblSendIP";
            this.lblSendIP.Size = new System.Drawing.Size(64, 16);
            this.lblSendIP.TabIndex = 6;
            this.lblSendIP.Text = "IP Address";
            this.lblSendIP.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSendPort
            // 
            this.lblSendPort.AutoSize = true;
            this.lblSendPort.Enabled = false;
            this.lblSendPort.Location = new System.Drawing.Point(109, 50);
            this.lblSendPort.Name = "lblSendPort";
            this.lblSendPort.Size = new System.Drawing.Size(26, 13);
            this.lblSendPort.TabIndex = 9;
            this.lblSendPort.Text = "Port";
            // 
            // txtSendIP
            // 
            this.txtSendIP.Enabled = false;
            this.txtSendIP.Location = new System.Drawing.Point(141, 74);
            this.txtSendIP.Name = "txtSendIP";
            this.txtSendIP.Size = new System.Drawing.Size(96, 20);
            this.txtSendIP.TabIndex = 4;
            this.txtSendIP.Text = "127.0.0.1";
            this.txtSendIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numSendPort
            // 
            this.numSendPort.Enabled = false;
            this.numSendPort.Location = new System.Drawing.Point(141, 48);
            this.numSendPort.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numSendPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSendPort.Name = "numSendPort";
            this.numSendPort.Size = new System.Drawing.Size(56, 20);
            this.numSendPort.TabIndex = 1;
            this.numSendPort.Value = new decimal(new int[] {
            9079,
            0,
            0,
            0});
            // 
            // grpServer
            // 
            this.grpServer.Controls.Add(this.chkSendPres);
            this.grpServer.Controls.Add(this.chkEcho);
            this.grpServer.Controls.Add(this.lblRecPort);
            this.grpServer.Controls.Add(this.cboRecProtocol);
            this.grpServer.Controls.Add(this.lblRecIP);
            this.grpServer.Controls.Add(this.txtRecIP);
            this.grpServer.Controls.Add(this.numRecPort);
            this.grpServer.Controls.Add(this.chkReceive);
            this.grpServer.Location = new System.Drawing.Point(8, 8);
            this.grpServer.Name = "grpServer";
            this.grpServer.Size = new System.Drawing.Size(247, 160);
            this.grpServer.TabIndex = 0;
            this.grpServer.TabStop = false;
            this.grpServer.Text = "Server";
            // 
            // chkSendPres
            // 
            this.chkSendPres.AutoSize = true;
            this.chkSendPres.Enabled = false;
            this.chkSendPres.Location = new System.Drawing.Point(6, 131);
            this.chkSendPres.Name = "chkSendPres";
            this.chkSendPres.Size = new System.Drawing.Size(204, 17);
            this.chkSendPres.TabIndex = 10;
            this.chkSendPres.Text = "Send new presented phrases to client";
            this.chkSendPres.UseVisualStyleBackColor = true;
            // 
            // chkEcho
            // 
            this.chkEcho.AutoSize = true;
            this.chkEcho.Enabled = false;
            this.chkEcho.Location = new System.Drawing.Point(6, 108);
            this.chkEcho.Name = "chkEcho";
            this.chkEcho.Size = new System.Drawing.Size(215, 17);
            this.chkEcho.TabIndex = 10;
            this.chkEcho.Text = "Echo received characters back to client";
            this.chkEcho.UseVisualStyleBackColor = true;
            // 
            // lblRecPort
            // 
            this.lblRecPort.AutoSize = true;
            this.lblRecPort.Enabled = false;
            this.lblRecPort.Location = new System.Drawing.Point(109, 50);
            this.lblRecPort.Name = "lblRecPort";
            this.lblRecPort.Size = new System.Drawing.Size(26, 13);
            this.lblRecPort.TabIndex = 9;
            this.lblRecPort.Text = "Port";
            // 
            // cboRecProtocol
            // 
            this.cboRecProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRecProtocol.Enabled = false;
            this.cboRecProtocol.FormattingEnabled = true;
            this.cboRecProtocol.Items.AddRange(new object[] {
            "TCP",
            "WebSockets"});
            this.cboRecProtocol.Location = new System.Drawing.Point(141, 21);
            this.cboRecProtocol.Name = "cboRecProtocol";
            this.cboRecProtocol.Size = new System.Drawing.Size(96, 21);
            this.cboRecProtocol.TabIndex = 8;
            // 
            // lblRecIP
            // 
            this.lblRecIP.Enabled = false;
            this.lblRecIP.Location = new System.Drawing.Point(71, 77);
            this.lblRecIP.Name = "lblRecIP";
            this.lblRecIP.Size = new System.Drawing.Size(64, 16);
            this.lblRecIP.TabIndex = 7;
            this.lblRecIP.Text = "IP Address";
            this.lblRecIP.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtRecIP
            // 
            this.txtRecIP.Enabled = false;
            this.txtRecIP.Location = new System.Drawing.Point(141, 74);
            this.txtRecIP.Name = "txtRecIP";
            this.txtRecIP.Size = new System.Drawing.Size(96, 20);
            this.txtRecIP.TabIndex = 5;
            this.txtRecIP.Text = "127.0.0.1";
            this.txtRecIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numRecPort
            // 
            this.numRecPort.Enabled = false;
            this.numRecPort.Location = new System.Drawing.Point(141, 48);
            this.numRecPort.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numRecPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRecPort.Name = "numRecPort";
            this.numRecPort.Size = new System.Drawing.Size(56, 20);
            this.numRecPort.TabIndex = 3;
            this.numRecPort.Value = new decimal(new int[] {
            9080,
            0,
            0,
            0});
            // 
            // chkReceive
            // 
            this.chkReceive.Location = new System.Drawing.Point(6, 19);
            this.chkReceive.Name = "chkReceive";
            this.chkReceive.Size = new System.Drawing.Size(137, 24);
            this.chkReceive.TabIndex = 2;
            this.chkReceive.Text = "Receive characters via";
            this.chkReceive.CheckedChanged += new System.EventHandler(this.chkReceive_CheckedChanged);
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(111, 319);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(192, 319);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            // 
            // OptionsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(272, 349);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.tabOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.OptionsForm_Closing);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.tabOptions.ResumeLayout(false);
            this.tpgAutomatic.ResumeLayout(false);
            this.grpAutomatic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSwitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStop)).EndInit();
            this.tpgPrevent.ResumeLayout(false);
            this.grpPrevent.ResumeLayout(false);
            this.tpgNetwork.ResumeLayout(false);
            this.grpClient.ResumeLayout(false);
            this.grpClient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSendPort)).EndInit();
            this.grpServer.ResumeLayout(false);
            this.grpServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecPort)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		// accessor
		public Options Settings
		{
			get { return _o; }
		}

		// options in
		private void OptionsForm_Load(object sender, System.EventArgs e)
		{
			// Automatic tab
			chkStop.Checked = _o.AutoStop;
			numStop.Value = _o.StopAfter;
			optStop.Checked = !_o.AutoQuit;
			optExit.Checked = _o.AutoQuit;
			chkSwitch.Checked = _o.AutoSwitch;
			numSwitch.Value = _o.SwitchAfter;

			// Prevention tab
			chkNoPunct.Checked = _o.NoPunctuation;
			chkNoNums.Checked = _o.NoNumbers;
			chkNoCaps.Checked = _o.NoCapitals;
			chkNoBksp.Checked = _o.NoBackspace;
			chkNoWordBksp.Checked = _o.NoWordBackspace;
			chkNoEnter.Checked = _o.NoEnter;

			// Network tab
			chkReceive.Checked = _o.ReceiveOnNet;
            cboRecProtocol.SelectedIndex = (_o.UseWebSockets ? 1 : 0);
			numRecPort.Value = _o.ReceivePort;
			txtRecIP.Text = _o.ReceiveIP;
            chkEcho.Checked = _o.Echo;
            chkSendPres.Checked = _o.SendPres;
            chkSend.Checked = _o.SendOnTcp;
            cboSendProtocol.SelectedIndex = 0;
            numSendPort.Value = _o.SendPort;
            txtSendIP.Text = _o.SendIP;
        }

		// options out
		private void OptionsForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Automatic tab
			_o.AutoStop = chkStop.Checked;
			_o.StopAfter = (int) numStop.Value;
			_o.AutoQuit = optExit.Checked;
			_o.AutoSwitch = chkSwitch.Checked;
			_o.SwitchAfter = (int) numSwitch.Value;

			// Prevention tab
			_o.NoPunctuation = chkNoPunct.Checked;
			_o.NoNumbers = chkNoNums.Checked;
			_o.NoCapitals = chkNoCaps.Checked;
			_o.NoBackspace = chkNoBksp.Checked;
			_o.NoWordBackspace = chkNoWordBksp.Checked;
			_o.NoEnter = chkNoEnter.Checked;			

			// Network tab
			_o.ReceiveOnNet = chkReceive.Checked;
            _o.UseWebSockets = (cboRecProtocol.SelectedIndex == 1);
			_o.ReceivePort = (int) numRecPort.Value;
			_o.ReceiveIP = txtRecIP.Text;
            _o.Echo = chkEcho.Checked;
            _o.SendPres = chkSendPres.Checked;
            _o.SendOnTcp = chkSend.Checked;
            _o.SendPort = (int) numSendPort.Value;
            _o.SendIP = txtSendIP.Text;
        }

		private void chkStop_CheckedChanged(object sender, System.EventArgs e)
		{
			numStop.Enabled = chkStop.Checked;
			optStop.Enabled = chkStop.Checked;
			optExit.Enabled = chkStop.Checked;
		}

		private void chkSwitch_CheckedChanged(object sender, System.EventArgs e)
		{
			numSwitch.Enabled = chkSwitch.Checked;
		}

		private void chkNoBksp_CheckedChanged(object sender, System.EventArgs e)
		{
			chkNoWordBksp.Checked = chkNoBksp.Checked;
			chkNoWordBksp.Enabled = !chkNoBksp.Checked;
		}

        private void chkReceive_CheckedChanged(object sender, System.EventArgs e)
        {
            cboRecProtocol.Enabled = chkReceive.Checked;
            lblRecPort.Enabled = chkReceive.Checked;
            numRecPort.Enabled = chkReceive.Checked;
            lblRecIP.Enabled = chkReceive.Checked;
            txtRecIP.Enabled = chkReceive.Checked;
            chkEcho.Enabled = chkReceive.Checked;
            chkSendPres.Enabled = chkReceive.Checked;
        }

        private void chkSend_CheckedChanged(object sender, System.EventArgs e)
		{
            cboSendProtocol.Enabled = chkSend.Checked;
            lblSendPort.Enabled = chkSend.Checked;
			numSendPort.Enabled = chkSend.Checked;
			lblSendIP.Enabled = chkSend.Checked;
			txtSendIP.Enabled = chkSend.Checked;
		}

	}
}