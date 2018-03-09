using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace TextTest
{
	public class AboutForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox picAbout;
		private System.Windows.Forms.TextBox txtAbout;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.LinkLabel lnkAbout;
		private System.ComponentModel.Container components = null;

		public AboutForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.picAbout = new System.Windows.Forms.PictureBox();
            this.txtAbout = new System.Windows.Forms.TextBox();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lnkAbout = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picAbout)).BeginInit();
            this.SuspendLayout();
            // 
            // picAbout
            // 
            this.picAbout.Image = ((System.Drawing.Image)(resources.GetObject("picAbout.Image")));
            this.picAbout.Location = new System.Drawing.Point(16, 16);
            this.picAbout.Name = "picAbout";
            this.picAbout.Size = new System.Drawing.Size(24, 24);
            this.picAbout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAbout.TabIndex = 0;
            this.picAbout.TabStop = false;
            // 
            // txtAbout
            // 
            this.txtAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAbout.Location = new System.Drawing.Point(56, 8);
            this.txtAbout.Multiline = true;
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.ReadOnly = true;
            this.txtAbout.Size = new System.Drawing.Size(144, 159);
            this.txtAbout.TabIndex = 1;
            this.txtAbout.TabStop = false;
            this.txtAbout.Text = "           TextTest\r\n        Version 2.8.4\r\n\r\nJacob O. Wobbrock, Ph.D.\r\nwobbrock@" +
    "uw.edu\r\n\r\n\r\nCopyright (C) 2006-2018\r\nUniversity of Washington\r\n\r\nCopyright (C) 2" +
    "004-2006\r\nCarnegie Mellon University";
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(96, 176);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Text = "OK";
            // 
            // lnkAbout
            // 
            this.lnkAbout.Location = new System.Drawing.Point(53, 79);
            this.lnkAbout.Name = "lnkAbout";
            this.lnkAbout.Size = new System.Drawing.Size(160, 16);
            this.lnkAbout.TabIndex = 3;
            this.lnkAbout.TabStop = true;
            this.lnkAbout.Text = "http://faculty.uw.edu/wobbrock/";
            this.lnkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAbout_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(266, 207);
            this.Controls.Add(this.lnkAbout);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.txtAbout);
            this.Controls.Add(this.picAbout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About TextTest";
            ((System.ComponentModel.ISupportInitialize)(this.picAbout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void lnkAbout_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://faculty.uw.edu/wobbrock/");
		}
        
    }
}
