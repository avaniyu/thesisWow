using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace TextTest
{
	public class ParticipantNumbering : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtAbout;
		private System.Windows.Forms.Button cmdOK;
        private NumericUpDown partNumber;
        private TextBox textBox2;
        private System.ComponentModel.Container components = null;

		public ParticipantNumbering()
		{
			InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParticipantNumbering));
            this.txtAbout = new System.Windows.Forms.TextBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.partNumber = new System.Windows.Forms.NumericUpDown();
            this.textBox2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.partNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAbout
            // 
            this.txtAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAbout.Location = new System.Drawing.Point(49, 12);
            this.txtAbout.Multiline = true;
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.ReadOnly = true;
            this.txtAbout.Size = new System.Drawing.Size(379, 159);
            this.txtAbout.TabIndex = 1;
            this.txtAbout.TabStop = false;
            this.txtAbout.Text = resources.GetString("txtAbout.Text");
            this.txtAbout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(201, 402);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // partNumber
            // 
            this.partNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.partNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.partNumber.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.partNumber.Location = new System.Drawing.Point(181, 240);
            this.partNumber.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.partNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.partNumber.MinimumSize = new System.Drawing.Size(120, 0);
            this.partNumber.Name = "partNumber";
            this.partNumber.Size = new System.Drawing.Size(120, 68);
            this.partNumber.TabIndex = 3;
            this.partNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.partNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(132, 203);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(220, 31);
            this.textBox2.TabIndex = 5;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Participant numbering:";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ParticipantNumbering
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.partNumber);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.txtAbout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParticipantNumbering";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Participant Numbering";
            ((System.ComponentModel.ISupportInitialize)(this.partNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void cmdOK_Click(object sender, EventArgs e)
        {
            System.Threading.Thread myThread = new System.Threading.Thread(new System.Threading.ThreadStart(OpenNewForm));
            myThread.Start();
            this.Close();
        }
        public static void OpenNewForm()
        {
            Application.Run(new MainForm());
        }

        public decimal ParticipantNumber
        {
            get { return partNumber.Value; }
        }
    }
}
