namespace TextTest
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.rtxPresented = new System.Windows.Forms.RichTextBox();
            this.staStatus = new System.Windows.Forms.StatusBar();
            this.pnlLog = new System.Windows.Forms.StatusBarPanel();
            this.pnlTest = new System.Windows.Forms.StatusBarPanel();
            this.pnlTask = new System.Windows.Forms.StatusBarPanel();
            this.rtxTranscribed = new System.Windows.Forms.RichTextBox();
            this.cmdNext = new System.Windows.Forms.Button();
            this.prgLogs = new System.Windows.Forms.ProgressBar();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniOpenPhrases = new System.Windows.Forms.ToolStripMenuItem();
            this.mniRenameLog = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTest = new System.Windows.Forms.ToolStripMenuItem();
            this.mniStartTest = new System.Windows.Forms.ToolStripMenuItem();
            this.mniStopTest = new System.Windows.Forms.ToolStripMenuItem();
            this.mniNextPhrase = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniTestOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniPracticeFlag = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTestFlag = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFontFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAnalyze = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAnalyzeLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAnalyzeGraphs = new System.Windows.Forms.ToolStripMenuItem();
            this.mniAnalyzeInputStream = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBibliography = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSampleXML = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniAboutApp = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pnlLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTask)).BeginInit();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // rtxPresented
            // 
            this.rtxPresented.BackColor = System.Drawing.SystemColors.Info;
            this.rtxPresented.DetectUrls = false;
            this.rtxPresented.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtxPresented.Enabled = false;
            this.rtxPresented.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxPresented.Location = new System.Drawing.Point(1, 24);
            this.rtxPresented.Multiline = false;
            this.rtxPresented.Name = "rtxPresented";
            this.rtxPresented.ReadOnly = true;
            this.rtxPresented.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxPresented.Size = new System.Drawing.Size(1067, 60);
            this.rtxPresented.TabIndex = 2;
            this.rtxPresented.TabStop = false;
            this.rtxPresented.Text = "";
            this.rtxPresented.WordWrap = false;
            this.rtxPresented.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtxPresented_MouseDown);
            this.rtxPresented.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rtxPresented_MouseUp);
            // 
            // staStatus
            // 
            this.staStatus.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.staStatus.Location = new System.Drawing.Point(1, 531);
            this.staStatus.Name = "staStatus";
            this.staStatus.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.pnlLog,
            this.pnlTest,
            this.pnlTask});
            this.staStatus.ShowPanels = true;
            this.staStatus.Size = new System.Drawing.Size(1067, 42);
            this.staStatus.TabIndex = 3;
            // 
            // pnlLog
            // 
            this.pnlLog.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.pnlLog.Icon = ((System.Drawing.Icon)(resources.GetObject("pnlLog.Icon")));
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Text = "no log";
            this.pnlLog.Width = 350;
            // 
            // pnlTest
            // 
            this.pnlTest.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.pnlTest.Icon = ((System.Drawing.Icon)(resources.GetObject("pnlTest.Icon")));
            this.pnlTest.Name = "pnlTest";
            this.pnlTest.Text = "Practice";
            this.pnlTest.Width = 350;
            // 
            // pnlTask
            // 
            this.pnlTask.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.pnlTask.Icon = ((System.Drawing.Icon)(resources.GetObject("pnlTask.Icon")));
            this.pnlTask.Name = "pnlTask";
            this.pnlTask.Text = "0";
            this.pnlTask.Width = 350;
            // 
            // rtxTranscribed
            // 
            this.rtxTranscribed.DetectUrls = false;
            this.rtxTranscribed.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtxTranscribed.Enabled = false;
            this.rtxTranscribed.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxTranscribed.Location = new System.Drawing.Point(1, 84);
            this.rtxTranscribed.Multiline = false;
            this.rtxTranscribed.Name = "rtxTranscribed";
            this.rtxTranscribed.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxTranscribed.Size = new System.Drawing.Size(1067, 60);
            this.rtxTranscribed.TabIndex = 0;
            this.rtxTranscribed.Text = "";
            this.rtxTranscribed.WordWrap = false;
            this.rtxTranscribed.SelectionChanged += new System.EventHandler(this.rtxTranscribed_SelectionChanged);
            this.rtxTranscribed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtxTranscribed_KeyDown);
            this.rtxTranscribed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtxTranscribed_KeyPress);
            this.rtxTranscribed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rtxTranscribed_MouseUp);
            // 
            // cmdNext
            // 
            this.cmdNext.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmdNext.Enabled = false;
            this.cmdNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdNext.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdNext.Location = new System.Drawing.Point(1, 144);
            this.cmdNext.Name = "cmdNext";
            this.cmdNext.Size = new System.Drawing.Size(1067, 40);
            this.cmdNext.TabIndex = 1;
            this.cmdNext.TabStop = false;
            this.cmdNext.Text = "Next";
            this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
            this.cmdNext.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmdNext_KeyPress);
            // 
            // prgLogs
            // 
            this.prgLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.prgLogs.Location = new System.Drawing.Point(1, 184);
            this.prgLogs.Name = "prgLogs";
            this.prgLogs.Size = new System.Drawing.Size(1067, 16);
            this.prgLogs.Step = 1;
            this.prgLogs.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgLogs.TabIndex = 4;
            this.prgLogs.UseWaitCursor = true;
            this.prgLogs.Visible = false;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuTest,
            this.mnuFormat,
            this.mnuAnalyze,
            this.mnuHelp});
            this.mnuMain.Location = new System.Drawing.Point(1, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1067, 24);
            this.mnuMain.TabIndex = 5;
            this.mnuMain.Text = "mnuMain";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniOpenPhrases,
            this.mniRenameLog,
            this.mniSeparator1,
            this.mniExitApp});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            this.mnuFile.DropDownOpening += new System.EventHandler(this.mnuFile_DropDownOpening);
            // 
            // mniOpenPhrases
            // 
            this.mniOpenPhrases.Name = "mniOpenPhrases";
            this.mniOpenPhrases.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.mniOpenPhrases.Size = new System.Drawing.Size(198, 22);
            this.mniOpenPhrases.Text = "Open P&hrases...";
            this.mniOpenPhrases.Click += new System.EventHandler(this.mniOpenPhrases_Click);
            // 
            // mniRenameLog
            // 
            this.mniRenameLog.Name = "mniRenameLog";
            this.mniRenameLog.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mniRenameLog.Size = new System.Drawing.Size(198, 22);
            this.mniRenameLog.Text = "&Rename Log...";
            this.mniRenameLog.Click += new System.EventHandler(this.mniRenameLog_Click);
            // 
            // mniSeparator1
            // 
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.Size = new System.Drawing.Size(195, 6);
            // 
            // mniExitApp
            // 
            this.mniExitApp.Name = "mniExitApp";
            this.mniExitApp.Size = new System.Drawing.Size(198, 22);
            this.mniExitApp.Text = "E&xit";
            this.mniExitApp.Click += new System.EventHandler(this.mniExitApp_Click);
            // 
            // mnuTest
            // 
            this.mnuTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniStartTest,
            this.mniStopTest,
            this.mniNextPhrase,
            this.mniSeparator2,
            this.mniTestOptions,
            this.mniSeparator3,
            this.mniPracticeFlag,
            this.mniTestFlag});
            this.mnuTest.Name = "mnuTest";
            this.mnuTest.Size = new System.Drawing.Size(40, 20);
            this.mnuTest.Text = "&Test";
            this.mnuTest.DropDownOpening += new System.EventHandler(this.mnuTest_DropDownOpening);
            // 
            // mniStartTest
            // 
            this.mniStartTest.Name = "mniStartTest";
            this.mniStartTest.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mniStartTest.Size = new System.Drawing.Size(168, 22);
            this.mniStartTest.Text = "Start &New";
            this.mniStartTest.Click += new System.EventHandler(this.mniStartTest_Click);
            // 
            // mniStopTest
            // 
            this.mniStopTest.Name = "mniStopTest";
            this.mniStopTest.ShortcutKeyDisplayString = "Esc";
            this.mniStopTest.Size = new System.Drawing.Size(168, 22);
            this.mniStopTest.Text = "&Stop";
            this.mniStopTest.Click += new System.EventHandler(this.mniStopTest_Click);
            // 
            // mniNextPhrase
            // 
            this.mniNextPhrase.Name = "mniNextPhrase";
            this.mniNextPhrase.ShortcutKeyDisplayString = "Enter";
            this.mniNextPhrase.Size = new System.Drawing.Size(168, 22);
            this.mniNextPhrase.Text = "Ne&xt";
            this.mniNextPhrase.Click += new System.EventHandler(this.mniNextPhrase_Click);
            // 
            // mniSeparator2
            // 
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.Size = new System.Drawing.Size(165, 6);
            // 
            // mniTestOptions
            // 
            this.mniTestOptions.Name = "mniTestOptions";
            this.mniTestOptions.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mniTestOptions.Size = new System.Drawing.Size(168, 22);
            this.mniTestOptions.Text = "&Options...";
            this.mniTestOptions.Click += new System.EventHandler(this.mniTestOptions_Click);
            // 
            // mniSeparator3
            // 
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.Size = new System.Drawing.Size(165, 6);
            // 
            // mniPracticeFlag
            // 
            this.mniPracticeFlag.Name = "mniPracticeFlag";
            this.mniPracticeFlag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.mniPracticeFlag.Size = new System.Drawing.Size(168, 22);
            this.mniPracticeFlag.Text = "&Practice";
            this.mniPracticeFlag.Click += new System.EventHandler(this.mniPracticeFlag_Click);
            // 
            // mniTestFlag
            // 
            this.mniTestFlag.Name = "mniTestFlag";
            this.mniTestFlag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.mniTestFlag.Size = new System.Drawing.Size(168, 22);
            this.mniTestFlag.Text = "&Test";
            this.mniTestFlag.Click += new System.EventHandler(this.mniTestFlag_Click);
            // 
            // mnuFormat
            // 
            this.mnuFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFontFormat});
            this.mnuFormat.Name = "mnuFormat";
            this.mnuFormat.Size = new System.Drawing.Size(57, 20);
            this.mnuFormat.Text = "F&ormat";
            // 
            // mniFontFormat
            // 
            this.mniFontFormat.Name = "mniFontFormat";
            this.mniFontFormat.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.mniFontFormat.Size = new System.Drawing.Size(147, 22);
            this.mniFontFormat.Text = "&Font...";
            this.mniFontFormat.Click += new System.EventHandler(this.mniFontFormat_Click);
            // 
            // mnuAnalyze
            // 
            this.mnuAnalyze.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniAnalyzeLogs,
            this.mniAnalyzeGraphs,
            this.mniAnalyzeInputStream});
            this.mnuAnalyze.Name = "mnuAnalyze";
            this.mnuAnalyze.Size = new System.Drawing.Size(60, 20);
            this.mnuAnalyze.Text = "&Analyze";
            this.mnuAnalyze.DropDownOpening += new System.EventHandler(this.mnuAnalyze_DropDownOpening);
            // 
            // mniAnalyzeLogs
            // 
            this.mniAnalyzeLogs.Name = "mniAnalyzeLogs";
            this.mniAnalyzeLogs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.mniAnalyzeLogs.Size = new System.Drawing.Size(188, 22);
            this.mniAnalyzeLogs.Text = "&Logs...";
            this.mniAnalyzeLogs.Click += new System.EventHandler(this.mniAnalyseLogs_Click);
            // 
            // mniAnalyzeGraphs
            // 
            this.mniAnalyzeGraphs.Name = "mniAnalyzeGraphs";
            this.mniAnalyzeGraphs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.mniAnalyzeGraphs.Size = new System.Drawing.Size(188, 22);
            this.mniAnalyzeGraphs.Text = "&Graphs...";
            this.mniAnalyzeGraphs.Click += new System.EventHandler(this.mniAnalyseGraphs_Click);
            // 
            // mniAnalyzeInputStream
            // 
            this.mniAnalyzeInputStream.Enabled = false;
            this.mniAnalyzeInputStream.Name = "mniAnalyzeInputStream";
            this.mniAnalyzeInputStream.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.mniAnalyzeInputStream.Size = new System.Drawing.Size(188, 22);
            this.mniAnalyzeInputStream.Text = "&Input Stream...";
            this.mniAnalyzeInputStream.Visible = false;
            this.mniAnalyzeInputStream.Click += new System.EventHandler(this.mniAnalyseInputStream_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniBibliography,
            this.mniSampleXML,
            this.mniSeparator4,
            this.mniAboutApp});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // mniBibliography
            // 
            this.mniBibliography.Name = "mniBibliography";
            this.mniBibliography.Size = new System.Drawing.Size(163, 22);
            this.mniBibliography.Text = "&Bibliography";
            this.mniBibliography.Click += new System.EventHandler(this.mniBibliography_Click);
            // 
            // mniSampleXML
            // 
            this.mniSampleXML.Name = "mniSampleXML";
            this.mniSampleXML.Size = new System.Drawing.Size(163, 22);
            this.mniSampleXML.Text = "Sample &XML Log";
            this.mniSampleXML.Click += new System.EventHandler(this.mniSampleXML_Click);
            // 
            // mniSeparator4
            // 
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.Size = new System.Drawing.Size(160, 6);
            // 
            // mniAboutApp
            // 
            this.mniAboutApp.Name = "mniAboutApp";
            this.mniAboutApp.Size = new System.Drawing.Size(163, 22);
            this.mniAboutApp.Text = "&About TextTest";
            this.mniAboutApp.Click += new System.EventHandler(this.mniAboutApp_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(122, 221);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(106, 90);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(806, 268);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(106, 90);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(17, 31);
            this.ClientSize = new System.Drawing.Size(1069, 573);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.prgLogs);
            this.Controls.Add(this.cmdNext);
            this.Controls.Add(this.rtxTranscribed);
            this.Controls.Add(this.staStatus);
            this.Controls.Add(this.rtxPresented);
            this.Controls.Add(this.mnuMain);
            this.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 300);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TextTest";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pnlLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTask)).EndInit();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.StatusBar staStatus;
        private System.Windows.Forms.StatusBarPanel pnlLog;
        private System.Windows.Forms.StatusBarPanel pnlTest;
        private System.Windows.Forms.StatusBarPanel pnlTask;
        private System.Windows.Forms.RichTextBox rtxPresented;
        private System.Windows.Forms.RichTextBox rtxTranscribed;
        private System.Windows.Forms.Button cmdNext;
        private System.Windows.Forms.ProgressBar prgLogs;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mniOpenPhrases;
        private System.Windows.Forms.ToolStripMenuItem mniRenameLog;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniExitApp;
        private System.Windows.Forms.ToolStripMenuItem mnuTest;
        private System.Windows.Forms.ToolStripMenuItem mniStartTest;
        private System.Windows.Forms.ToolStripMenuItem mniStopTest;
        private System.Windows.Forms.ToolStripMenuItem mniNextPhrase;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniPracticeFlag;
        private System.Windows.Forms.ToolStripMenuItem mniTestFlag;
        private System.Windows.Forms.ToolStripMenuItem mnuFormat;
        private System.Windows.Forms.ToolStripMenuItem mnuAnalyze;
        private System.Windows.Forms.ToolStripMenuItem mniAnalyzeLogs;
        private System.Windows.Forms.ToolStripMenuItem mniAnalyzeGraphs;
        private System.Windows.Forms.ToolStripMenuItem mniAnalyzeInputStream;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mniBibliography;
        private System.Windows.Forms.ToolStripMenuItem mniSampleXML;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniAboutApp;
        private System.Windows.Forms.ToolStripMenuItem mniTestOptions;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniFontFormat;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}
