namespace TextTest
{
    partial class OutputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputForm));
            this.chklstOptions = new System.Windows.Forms.CheckedListBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.lnklblSelectAll = new System.Windows.Forms.LinkLabel();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chklstOptions
            // 
            this.chklstOptions.CheckOnClick = true;
            this.chklstOptions.FormattingEnabled = true;
            this.chklstOptions.Items.AddRange(new object[] {
            "Main measures for each trial (*.csv)",
            "Optimal alignments (*.align.txt)",
            "Character-level error table (*.table.csv)",
            "Character-level confusion matrix (*.matrix.csv)",
            "Optimal alignments with input streams (*.salign.txt)",
            "Character-level error table from input streams (*.stable.csv)",
            "Character-level confusion matrix from input streams (*.smatrix.csv)"});
            this.chklstOptions.Location = new System.Drawing.Point(12, 29);
            this.chklstOptions.Name = "chklstOptions";
            this.chklstOptions.Size = new System.Drawing.Size(338, 124);
            this.chklstOptions.TabIndex = 1;
            this.chklstOptions.SelectedIndexChanged += new System.EventHandler(this.chklstOptions_SelectedIndexChanged);
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(9, 9);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(149, 13);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "Select the desired output files.";
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Enabled = false;
            this.cmdOK.Location = new System.Drawing.Point(194, 159);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(275, 159);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // lnklblSelectAll
            // 
            this.lnklblSelectAll.AutoSize = true;
            this.lnklblSelectAll.Location = new System.Drawing.Point(12, 156);
            this.lnklblSelectAll.Name = "lnklblSelectAll";
            this.lnklblSelectAll.Size = new System.Drawing.Size(51, 13);
            this.lnklblSelectAll.TabIndex = 2;
            this.lnklblSelectAll.TabStop = true;
            this.lnklblSelectAll.Text = "Select All";
            this.lnklblSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblSelectAll_LinkClicked);
            // 
            // cmdHelp
            // 
            this.cmdHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.cmdHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.cmdHelp.Image = ((System.Drawing.Image)(resources.GetObject("cmdHelp.Image")));
            this.cmdHelp.Location = new System.Drawing.Point(325, 4);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(25, 23);
            this.cmdHelp.TabIndex = 3;
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // OutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 191);
            this.Controls.Add(this.lnklblSelectAll);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.chklstOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "OutputForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Output Options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OutputForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chklstOptions;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.LinkLabel lnklblSelectAll;
        private System.Windows.Forms.Button cmdHelp;
    }
}