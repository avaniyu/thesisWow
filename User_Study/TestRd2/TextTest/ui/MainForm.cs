using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using WobbrockLib.Net;
using WobbrockLib.Extensions;

namespace TextTest
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region Fields

        private OptionsForm.Options _o;

        private Phrases _phrases;

        private string _fileNoExt; // the full file path without a file extension

        private SessionData _sd; // the current session being built (one test)
        private TrialData _td; // the current trial being built (one phrase)

        private int _bkspSelLength;

        private NetListener _listener;
        private NetSender _sender;

        #endregion

        #region Start & Stop

        /// <summary>
        /// The constructor for the main TextTest form.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            SetControlHeights(this.Font);
            SetMinFormHeight();

            _o = new OptionsForm.Options(); // default options
            _phrases = new Phrases();

            _fileNoExt = String.Empty;
            _bkspSelLength = 0;
        }

        /// <summary>
        /// Sets the heights of the controls on the form based on the
        /// supplied font.
        /// </summary>
        /// <param name="f">The Font from which to set the controls' sizes.</param>
        private void SetControlHeights(Font f)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                Control ctl = (Control) Controls[i];
                if (ctl is RichTextBox)
                    ctl.Height = (int) (f.Height * 1.50);
                else if (!(ctl is ProgressBar))
                    ctl.Height = (int) (f.Height * 1.25);
            }
        }

        /// <summary>
        /// Set the focus on the transcribed rich text box upon startup.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_Load(object sender, System.EventArgs e)
        {
            rtxTranscribed.Focus();
        }

        /// <summary>
        /// When the main form is being closed, we must be sure that any currently
        /// open tests are terminated properly. So we first stop the test, and then
        /// we can close safely without losing our log data.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_sender != null && _sender.IsConnected)
                _sender.Disconnect();
            if (_listener != null && _listener.IsListening)
                _listener.Stop();
            if (_sd != null)
                mniStopTest_Click(this, EventArgs.Empty);
        }

        #endregion

        #region Rich Text Box

        /// <summary>
        /// Intercepts a handful of key presses and prevents them. These are generally not 
        /// prevented based on user preferences (except backspace), but prevented to maintain 
        /// the integrity of the user test data.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void rtxTranscribed_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (_td == null) // no active trial
                return;

            // if we have a selected region of text, then any chars that
            // come in will replace that selection, and we can't have that. we
            // only allow selected regions to support word-level backspacing.
            // thus, the only key that can be permitted to replace a selection
            // is backspace. this also prevents multiple words being selected
            // at once using repeated Ctrl+Shit+Left sequences. 
            if (rtxTranscribed.SelectionLength > 0 && e.KeyCode != Keys.Back)
            {
                rtxTranscribed.Select(rtxTranscribed.TextLength, 0);
            }

            switch (e.KeyCode)
            {
                case Keys.Menu: // Alt
                case Keys.Tab:
                    e.Handled = true;
                    break;

                case Keys.Home:
                case Keys.End:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Delete:
                case Keys.Insert:
                    e.Handled = true; // prevent
                    break;

                case Keys.Back:
                    if (rtxTranscribed.SelectionLength > 0)
                    {
                        if (_o.NoBackspace)
                        {
                            // if we're not allowing backspace and we've got
                            // a text selection, restore the cursor to the end.
                            rtxTranscribed.Select(rtxTranscribed.TextLength, 0);
                        }
                        else if (!_o.NoWordBackspace)
                        {
                            // set this flag here to be used in rtxTranscribed_KeyPress.
                            // we can't read it there directly because it is lost by the 
                            // time that event is fired after this one.
                            _bkspSelLength = rtxTranscribed.SelectionLength;
                        }
                    }
                    // don't let Ctrl+Bksp or Alt+Bksp through. Shift+Bksp is just Bksp.
                    e.Handled = _o.NoBackspace || e.Control || e.Alt;
                    break;

                case Keys.Enter: // advance the trial
                    if (!_o.NoEnter)
                        mniNextPhrase_Click(this, EventArgs.Empty);
                    e.Handled = true;
                    break;

                case Keys.Escape: // stop the test
                    mniStopTest_Click(this, EventArgs.Empty);
                    e.Handled = true;
                    break;

                // the left arrow is only allowed if Ctrl+Shift are held down when 
                // the left arrow is pressed. this is to enable word-level backspacing.
                // here we honor the preference to disable word-level backspaces.
                case Keys.Left:
                    e.Handled = _o.NoWordBackspace || !(e.Control && e.Shift);
                    break;
            }
        }

        /// <summary>
        /// Handles preventing or allowing certain characters in the Rich Text Box
        /// destination field based on the user's preferences, and logs permitted
        /// characters.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void rtxTranscribed_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\t':      // Tab
                case '\n':      // Enter
                case '\r':      // Enter
                case '\x1B':    // Esc (27)
                    e.Handled = true;
                    break;

                case '\x7F':    // Ctrl+Bksp (127)
                    e.Handled = true;
                    break;

                case '\b':      // Backspace (8)
                    e.Handled = _o.NoBackspace;
                    break;

                default:        // some prevention preferences
                    if (_o.NoCapitals && Char.IsUpper(e.KeyChar))
                    {
                        char chLow = Char.ToLower(e.KeyChar);
                        rtxTranscribed.Text += chLow;
                        rtxTranscribed.Select(rtxTranscribed.TextLength, 0);
                        _td.Add(new EntryData(chLow, TimeEx.NowTicks)); // forced to lowercase
                        e.Handled = true;
                    }
                    if (_o.NoNumbers && Char.IsDigit(e.KeyChar))
                    {
                        e.Handled = true;
                    }
                    if (_o.NoPunctuation && (Char.IsPunctuation(e.KeyChar) || Char.IsSymbol(e.KeyChar)))
                    {
                        e.Handled = true;
                    }
                    break;
            }

            // log the character if it wasn't prevented
            if (!e.Handled)
            {
                // all regular characters are logged here. we use a loop to handle
                // word-level backspacing where a word is selected and then backspaced
                // in a single key-press. for analysis reasons, such word-level backspaces
                // must be logged as individual backspaces, but their identical timestamps 
                // belie their true nature as originating from a single keypress.
                do
                {
                    _td.Add(new EntryData(e.KeyChar, TimeEx.NowTicks));

                    // send the character over TCP if desired
                    if (_o.SendOnTcp
                        && _sender != null
                        && _sender.IsConnected)
                    {
                        _sender.Send(e.KeyChar.ToString());
                    }
                } while (--_bkspSelLength > 0);
                _bkspSelLength = 0; // reset
            }
        }

        /// <summary>
        /// Keeps the text cursor at the end of the transcribed text, except for the case
        /// where the selection highlight encompasses some point in the text to the end
        /// of the text. This exception is made so that word-level backspacing 
        /// (Ctrl+Shift+Left, Backspace) can be allowed. Otherwise, this handles mouse clicking,
        /// arrow keys, Home and End keys, Page Up and Page Down keys, and other ways of changing 
        /// the current selection cursor.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void rtxTranscribed_SelectionChanged(object sender, System.EventArgs e)
        {
            if (rtxTranscribed.SelectionStart + rtxTranscribed.SelectionLength != rtxTranscribed.TextLength)
            {
                rtxTranscribed.Select(rtxTranscribed.TextLength, 0);
            }
        }

        /// <summary>
        /// If the mouse is used to drag over the text, it can select the text as long as 
        /// the selection meets the criteria in rtxTranscribed_SelectionChanged, above. 
        /// But obviously we don't want the mouse to have this power, so upon mouse-up,
        /// we clear the selection just-made and put the cursor again at the end.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void rtxTranscribed_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            rtxTranscribed.Select(rtxTranscribed.TextLength, 0);
        }

        /// <summary>
        /// If the presented text field is clicked, put the focus back on the transcribed
        /// text field.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void rtxPresented_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            rtxTranscribed.Focus();
        }

        /// <summary>
        /// If the presented text field is clicked, put the focus back on the transcribed
        /// text field.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void rtxPresented_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            rtxTranscribed.Focus();
        }

        #endregion

        #region Network Events

        /// <summary>
		/// Event handler that fires whenever data is received over the network. This event 
		/// handler programmatically adds text to the transcription. It does not use
		/// SendKeys and send the keys through the low-level input stream because it
		/// would have to set the focus to do that, stealing it from another application
		/// that might have it and be sending data to us.
		/// </summary>
		/// <remarks>
		/// <b>Warning.</b> This method assumes that there is no word-level backspacing
		/// available in a remote method, since it only sends char data to TextTest and
		/// no special key-code combinations like the Ctrl+Shift+Left required to set 
		/// up a word-level erasure.
		/// </remarks>
		/// <param name="sender">The sender of this event.</param>
		/// <param name="e">The arguments for this event.</param>
		private void OnNetworkDataReceived(object sender, NetListener.NetEventArgs e)
        {
            if (!rtxTranscribed.Enabled)
                return; // idiot check

            for (int i = 0; i < e.Data.Length; i++)
            {
                char ch = e.Data[i];

                bool handled = false;
                switch (ch)
                {
                    case '\t': // Tab
                        handled = true;
                        break;

                    case '\n': // Enter
                    case '\r': // Enter
                        if (!_o.NoEnter)
                            mniNextPhrase_Click(this, EventArgs.Empty);
                        handled = true;
                        break;

                    case '\x1B': // Esc (27)
                        mniStopTest_Click(this, EventArgs.Empty);
                        handled = true;
                        break;

                    case '\b': // Backspace (8)
                        handled = (_o.NoBackspace || rtxTranscribed.TextLength == 0);
                        break;

                    default: // prevention
                        if (_o.NoCapitals && Char.IsUpper(ch))
                        {
                            handled = true;
                            char chLow = Char.ToLower(ch);
                            rtxTranscribed.Text += chLow;
                            rtxTranscribed.Select(rtxTranscribed.TextLength, 0);
                            _td.Add(new EntryData(chLow, TimeEx.NowTicks)); // forced to lowercase
                        }
                        if (_o.NoNumbers && Char.IsDigit(ch))
                        {
                            handled = true;
                        }
                        if (_o.NoPunctuation && (Char.IsPunctuation(ch) || Char.IsSymbol(ch)))
                        {
                            handled = true;
                        }
                        break;
                }
                // display and log the character
                if (!handled)
                {
                    if (ch == '\b')
                    {
                        rtxTranscribed.Text = rtxTranscribed.Text.Substring(0, rtxTranscribed.TextLength - 1);
                    }
                    else // the character is added here
                    {
                        rtxTranscribed.Text += ch;
                    }
                    rtxTranscribed.Select(rtxTranscribed.TextLength, 0); // keep cursor at the end
                    _td.Add(new EntryData(ch, TimeEx.NowTicks));

                    // echo character back to sending client?
                    if (_o.ReceiveOnNet
                        && _o.Echo
                        && _listener != null
                        && _listener.IsConnected)
                    {
                        _listener.Send(ch.ToString());
                    }
                }
            }
        }

        #endregion

        #region File Menu

        /// <summary>
        /// Enables or disables the Rename Log menu item on the File menu based on
        /// whether or not there is a test currently running. If there is a test
        /// running, the log is open and cannot be renamed. It can be renamed once
        /// the test has finished.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mnuFile_DropDownOpening(object sender, EventArgs e)
        {
            mniRenameLog.Enabled = (_fileNoExt != String.Empty && _td == null);
        }

        /// <summary>
        /// Allows the user to open a custom phrase file of their choosing. The
        /// phrase file must a text file with a single phrase on each line.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniOpenPhrases_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Phrases";
            dlg.AddExtension = true;
            dlg.DefaultExt = "txt";
            dlg.Filter = "Phrase Files (*.txt)|*.txt";
            dlg.InitialDirectory = Application.ExecutablePath;
            dlg.Multiselect = false;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (_phrases.Load(dlg.FileName))
                {
                    MessageBox.Show(this, "Phrases loaded successfully.", "Phrases Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "Unable to load phrases. Default phrases will be re-loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _phrases.Load();
                }
            }
        }

        /// <summary>
        /// Allows the user to rename the current or most recent log file. The log file must 
        /// closed in order for this menu item to be enabled.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniRenameLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Rename Log";
            dlg.AddExtension = true;
            dlg.DefaultExt = "xml";
            dlg.Filter = "Log Files (*.xml)|*.xml";
            dlg.OverwritePrompt = true;
            dlg.InitialDirectory = Path.GetDirectoryName(_fileNoExt);
            dlg.FileName = Path.GetFileName(_fileNoExt + ".xml");

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (dlg.FileName != _fileNoExt + ".xml")
                {
                    try
                    {
                        if (File.Exists(dlg.FileName))
                            File.Delete(dlg.FileName);
                        if (File.Exists(_fileNoExt + ".xml"))
                            File.Move(_fileNoExt + ".xml", dlg.FileName);

                        _fileNoExt = String.Format("{0}\\{1}", Path.GetDirectoryName(dlg.FileName), Path.GetFileNameWithoutExtension(dlg.FileName));
                        pnlLog.Text = Path.GetFileName(_fileNoExt + ".xml");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Handler for the File &gt; Exit menu item. Closes the application.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniExitApp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Test Menu

        /// <summary>
        /// Handles the popup event for the Test menu. Used to set the practice/testing
        /// radio check mark on those menu items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTest_DropDownOpening(object sender, EventArgs e)
        {
            mniStartTest.Enabled = (_sd == null);
            mniStopTest.Enabled = (_sd != null && _td != null);
            mniNextPhrase.Enabled = (_sd != null);

            mniTestOptions.Enabled = (_sd == null);

            mniPracticeFlag.Enabled = (_sd != null);
            mniTestFlag.Enabled = (_sd != null);
        }

        /// <summary>
        /// Starts a new test, resetting values and opening a new log.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniStartTest_Click(object sender, EventArgs e)
        {
            mniPracticeFlag.Checked = true;
            mniTestFlag.Checked = false;
            cmdNext.Text = "Start";
            cmdNext.Enabled = true;
            cmdNext.Focus(); // set the focus on the button so "Enter" starts a new trial

            _fileNoExt = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), Environment.TickCount);
            _sd = new SessionData(TimeEx.NowTicks); // make a new session

            UpdateStatusBar();
        }

        /// <summary>
        /// Stops the current test in progress, closing the test log and 
        /// clearing appropriate fields.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniStopTest_Click(object sender, EventArgs e)
        {
            // important to disable this up front to prevent network 
            // characters from getting through after a test is done.
            rtxTranscribed.Enabled = false;
            rtxPresented.Enabled = false;
            cmdNext.Enabled = false;

            if (_sd != null)
            {
                if (_td != null)
                {
                    if (_td.NumEntries > 0) // ignore an empty last trial
                    {
                        _td.Transcribed = rtxTranscribed.Text;
                        Debug.Assert(_td.VerifyStream(), "Input stream and transcribed string mismatch!");
                        _td.IsTesting = mniTestFlag.Checked;
                        _sd.Add(_td); // add the final trial
                    }
                    _td = null; // clear since test is over
                }

                XmlTextWriter xWriter = new XmlTextWriter(_fileNoExt + ".xml", Encoding.UTF8);
                _sd.WriteXmlHeader(xWriter);
                _sd.WriteXmlFooter(xWriter);
                ResultsForm frm = new ResultsForm(_sd, Path.GetFileName(_fileNoExt + ".xml"));
                frm.ShowDialog(this);
                _sd = null; // clear the session
            }

            rtxPresented.Clear();
            rtxTranscribed.Clear();

            UpdateStatusBar();

            // send to client we're done as '\0'
            if (_o.ReceiveOnNet
                && _o.SendPres
                && _listener != null
                && _listener.IsConnected)
            {
                char zero = '\0';
                _listener.Send(zero.ToString());
            }
        }

        /// <summary>
        /// Click handler for the command button "Next." Clicking this button is
        /// synonymous with clicking the Test &gt; Next menu item, and so it is 
        /// routed through that.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void cmdNext_Click(object sender, System.EventArgs e)
        {
            mniNextPhrase_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// If the focus is shifted to the "Next" button, it is possible that 'Esc'
        /// won't be caught and the test won't be terminated. This handler enables
        /// 'Esc' to be caught and handled.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        /// <remarks>Note that 'Enter' does not have to be caught because pressing
        /// 'Enter' when a button has the focus effectively clicks that button
        /// anyway.</remarks>
        private void cmdNext_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1B') // Esc
            {
                mniStopTest_Click(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Menu handler for the Test &gt; Next menu item. This menu item is 
        /// synonymous in function with the cmdNext button. This function 
        /// essentially banks the previous task and initiates the next one.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        Timer timer = new Timer();
        int timer_interval = 6; // 30x100 = 3000 ms to display presented text
        private void mniNextPhrase_Click(object sender, EventArgs e)
        {
            if (_o.AutoStop && (_td != null && _td.TrialNo == _o.StopAfter))
            {
                MessageBox.Show(this, "Test complete!", "Test Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mniStopTest_Click(this, EventArgs.Empty); // shows graphs

                if (_o.AutoQuit)
                {
                    DialogResult dr = MessageBox.Show(this, "The application will now exit.", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.OK)
                        this.Close(); // exit app   
                }
            }
            else // no auto-stopping
            {
                if (_td == null) // Start (not Next) was clicked for first trial
                {
                    cmdNext.Text = "Next";
                    rtxPresented.Visible = true;
                    rtxPresented.Enabled = true;
                    rtxTranscribed.Visible = false;
                    cmdNext.Enabled = false;
                }
                else // submit the end of the previous trial
                {
                    _td.Transcribed = rtxTranscribed.Text;
                    Debug.Assert(_td.VerifyStream(), "The input stream and transcribed string do not match.");
                    _td.IsTesting = mniTestFlag.Checked;
                    _sd.Add(_td); // add the last task

                    if (_o.AutoSwitch && _td.TrialNo == _o.SwitchAfter)
                    {
                        mniTestFlag.Checked = true;
                        mniPracticeFlag.Checked = false;
                    }
                }

                // set up the next phrase
                string presented = _phrases.GetRandomPhrase(_o.NoCapitals); //derandomize here
                rtxPresented.Text = presented;
                rtxPresented.Visible = true;
                rtxTranscribed.Visible = false;
                rtxTranscribed.Clear();
                cmdNext.Enabled = false;

                // create the new trial data
                _td = new TrialData(_td == null ? 1 : _td.TrialNo + 1, presented);

                // update the status bar panes
                UpdateStatusBar();

                // send this phrase over the network
                if (_o.ReceiveOnNet
                    && _o.SendPres
                    && _listener != null
                    && _listener.IsConnected)
                {
                    _listener.Send(presented);
                }

                timer.Interval = timer_interval;
                timer.Tick += new EventHandler(timer_Tick);
                prgMemorize.Maximum = timer_interval * 100;
                timer.Start();
            }
        }

        /// <summary>
        /// hide presented text to force participant type from memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, System.EventArgs e)
        {
            if (prgMemorize.Value != timer_interval * 100)
            {
                prgMemorize.Value++;
            }else
            {
                prgMemorize.Value = 0;
                rtxPresented.Visible = false;
                rtxTranscribed.Visible = true;
                rtxTranscribed.Enabled = true;
                rtxTranscribed.Focus();
                cmdNext.Enabled = true;
                timer.Stop();
            }
        }
        
        /// <summary>
        /// Shows the options for this application. The OptionsForm defines what
        /// options are available, and exports them in a public structure.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniTestOptions_Click(object sender, EventArgs e)
        {
            OptionsForm dlg = new OptionsForm(_o);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                // stop any sending or listening
                if (_sender != null && _sender.IsConnected)
                    _sender.Disconnect();
                if (_listener != null && _listener.IsListening)
                    _listener.Stop();

                // set our options reference
                _o = dlg.Settings;

                // potentially start receiving as a network server
                if (_o.ReceiveOnNet)
                {
                    if (_o.UseWebSockets)
                        _listener = new WebSocketListener(this); // ws
                    else
                        _listener = new NetListener(this); // tcp
                    _listener.NetDataEvent += new NetListener.NetEventHandler(OnNetworkDataReceived);

                    _listener.IP = _o.ReceiveIP;
                    _listener.Port = _o.ReceivePort;
                    _listener.Start();
                }

                // potentially start sending as a network client
                if (_o.SendOnTcp)
                {
                    _sender = new NetSender();
                    _sender.IP = _o.SendIP;
                    _sender.Port = _o.SendPort;
                    _sender.Connect();
                }
            }
        }

        /// <summary>
        /// Menu handler for the Test &gt; Practice menu item.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniPracticeFlag_Click(object sender, EventArgs e)
        {
            mniPracticeFlag.Checked = true;
            mniTestFlag.Checked = false;
            UpdateStatusBar();
        }

        /// <summary>
        /// Menu handler for the Test &gt; Test menu item.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniTestFlag_Click(object sender, EventArgs e)
        {
            mniPracticeFlag.Checked = false;
            mniTestFlag.Checked = true;
            UpdateStatusBar();
        }

        /// <summary>
        /// Updates all three panes of the StatusBar based on the current state
        /// of the test. This includes updating both icons and text.
        /// </summary>
        private void UpdateStatusBar()
        {
            // update the log panel
            if (_sd != null)
            {
                pnlLog.Text = Path.GetFileName(_fileNoExt + ".xml");
                pnlLog.Icon = new Icon(typeof(MainForm), "rsc.book01b.ico");
            }
            else
            {
                pnlLog.Text = (_fileNoExt == String.Empty) ? "no log" : Path.GetFileName(_fileNoExt + ".xml");
                pnlLog.Icon = new Icon(typeof(MainForm), "rsc.book01a.ico");
            }

            // update the practice/test panel
            if (mniTestFlag.Checked)
            {
                pnlTest.Text = "Test";
                pnlTest.Icon = new Icon(typeof(MainForm), "rsc.lighton.ico");
            }
            else
            {
                pnlTest.Text = "Practice";
                pnlTest.Icon = new Icon(typeof(MainForm), "rsc.lightoff.ico");
            }

            // update the task number panel
            if (_o.AutoStop)
            {
                pnlTask.Text = String.Format("{0} of {1}", (_td != null) ? _td.TrialNo : 0, _o.StopAfter);
            }
            else
            {
                pnlTask.Text = (_td != null) ? _td.TrialNo.ToString() : "0";
            }
        }

        #endregion

        #region Format Menu

        /// <summary>
        /// Present a font dialog and allow users to choose only fixed width fonts.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniFontFormat_Click(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            dlg.Font = this.Font;
            dlg.FixedPitchOnly = true;
            dlg.ShowApply = false;
            dlg.ShowEffects = false;
            dlg.ShowColor = false;
            dlg.AllowScriptChange = false;
            dlg.AllowVerticalFonts = false;

            // iterate through the form's children and set their fonts and sizes
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                rtxPresented.Font = dlg.Font;
                rtxTranscribed.Font = dlg.Font;
                rtxPresented.Height = (int) (1.50 * dlg.Font.Height);
                rtxTranscribed.Height = (int) (1.50 * dlg.Font.Height);
                this.SetMinFormHeight();
            }
        }

        /// <summary>
        /// Sets the form's minimum height based on the heights of
        /// all the controls within it. This ensures that all controls
        /// will remain showing even after resizing.
        /// </summary>
        private void SetMinFormHeight()
        {
            int formHeight = SystemInformation.CaptionHeight;
            formHeight += SystemInformation.MenuHeight;
            formHeight += rtxPresented.Top;
            formHeight += rtxPresented.Height;
            formHeight += rtxTranscribed.Height;
            formHeight += cmdNext.Height;
            formHeight += prgLogs.Height;
            formHeight += staStatus.Height;
            this.MinimumSize = new Size(0, formHeight + 20);
        }

        #endregion

        #region Analyze Menu

        /// <summary>
        /// Event handler for when the Analyze drop-down menu is about to show.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAnalyze_DropDownOpening(object sender, EventArgs e)
        {
            mniAnalyzeLogs.Enabled = (_sd == null);
            mniAnalyzeGraphs.Enabled = (_sd == null);
            mniAnalyzeInputStream.Enabled = (_sd == null);
        }

        /// <summary>
        /// Read in one or more XML logs and write out their results to CSV files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniAnalyseLogs_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DefaultExt = "xml";
            ofd.Filter = "Log files (*.xml)|*.xml";
            ofd.Title = "Analyze Log Files";
            ofd.Multiselect = true;
            ofd.RestoreDirectory = true;
            ofd.SupportMultiDottedExtensions = true;

            DialogResult result = ofd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                OutputForm frm = new OutputForm();
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    prgLogs.Maximum = ofd.FileNames.Length * frm.NumSelected;
                    prgLogs.Value = 0;
                    prgLogs.Visible = true;

                    for (int i = 0; i < ofd.FileNames.Length; i++)
                    {
                        SessionData sd = new SessionData();
                        XmlTextReader reader = new XmlTextReader(new StreamReader(ofd.FileNames[i]));
                        if (sd.ReadFromXml(reader))
                        {
                            string pathNoExt = String.Format("{0}\\{1}", Path.GetDirectoryName(ofd.FileNames[i]), Path.GetFileNameWithoutExtension(ofd.FileNames[i]));

                            OutputForm.OptionsFlags flag;
                            if ((flag = (frm.Options & OutputForm.OptionsFlags.MainResults)) != 0)
                            {
                                StreamWriter writer = new StreamWriter(pathNoExt + frm.GetExtensionFor(flag));
                                sd.WriteResultsToTxt(writer);
                                prgLogs.Value++;
                            }
                            if ((flag = (frm.Options & OutputForm.OptionsFlags.OptimalAlignments)) != 0)
                            {
                                StreamWriter writer = new StreamWriter(pathNoExt + frm.GetExtensionFor(flag));
                                sd.WriteOptimalAlignments(writer, false);
                                prgLogs.Value++;
                            }
                            if ((flag = (frm.Options & OutputForm.OptionsFlags.CharacterErrorTable)) != 0)
                            {
                                StreamWriter writer = new StreamWriter(pathNoExt + frm.GetExtensionFor(flag));
                                sd.WriteCharacterErrorTable(writer, false);
                                prgLogs.Value++;
                            }
                            if ((flag = (frm.Options & OutputForm.OptionsFlags.ConfusionMatrix)) != 0)
                            {
                                StreamWriter writer = new StreamWriter(pathNoExt + frm.GetExtensionFor(flag));
                                sd.WriteConfusionMatrix(writer, false);
                                prgLogs.Value++;
                            }
                            if ((flag = (frm.Options & OutputForm.OptionsFlags.OptimalAlignmentsWithStreams)) != 0)
                            {
                                StreamWriter writer = new StreamWriter(pathNoExt + frm.GetExtensionFor(flag));
                                sd.WriteOptimalAlignments(writer, true);
                                prgLogs.Value++;
                            }
                            if ((flag = (frm.Options & OutputForm.OptionsFlags.CharacterErrorTableFromStreams)) != 0)
                            {
                                StreamWriter writer = new StreamWriter(pathNoExt + frm.GetExtensionFor(flag));
                                sd.WriteCharacterErrorTable(writer, true);
                                prgLogs.Value++;
                            }
                            if ((flag = (frm.Options & OutputForm.OptionsFlags.ConfusionMatrixFromStreams)) != 0)
                            {
                                StreamWriter writer = new StreamWriter(pathNoExt + frm.GetExtensionFor(flag));
                                sd.WriteConfusionMatrix(writer, true);
                                prgLogs.Value++;
                            }
                        }
                    }
                    MessageBox.Show(this, "Output file(s) complete.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    prgLogs.Visible = false;
                }
            }
        }

        /// <summary>
        /// Graph the main WPM and error rate results from an XML log file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniAnalyseGraphs_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DefaultExt = "xml";
            ofd.Filter = "Log files (*.xml)|*.xml";
            ofd.Title = "Graph Log File";
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.SupportMultiDottedExtensions = true;

            DialogResult result = ofd.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                SessionData sd = new SessionData();
                XmlTextReader reader = new XmlTextReader(new StreamReader(ofd.FileName));
                if (reader != null && sd.ReadFromXml(reader))
                {
                    ResultsForm frm = new ResultsForm(sd, Path.GetFileName(ofd.FileName));
                    frm.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniAnalyseInputStream_Click(object sender, EventArgs e)
        {
            // TODO: Display a custom dialog for P and IS
            // and include buttons for adding custom
            // characters to IS like backspace
        }

        #endregion

        #region Help Menu

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniBibliography_Click(object sender, EventArgs e)
        {
            string path = String.Empty;
            StreamReader reader = null;
            StreamWriter writer = null;

            bool success = true;
            try
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TextTest.rsc.bibliography.txt");
                reader = new StreamReader(stream, Encoding.ASCII);
                path = String.Format("{0}\\bibliography.txt", Directory.GetCurrentDirectory());
                writer = new StreamWriter(path, false, Encoding.ASCII);

                string line;
                while ((line = reader.ReadLine()) != null)
                    writer.WriteLine(line);
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex);
                success = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (writer != null)
                    writer.Close();
            }

            if (success)
            {
                Process.Start("notepad.exe", path);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniSampleXML_Click(object sender, EventArgs e)
        {
            string path = String.Empty;
            StreamReader reader = null;
            StreamWriter writer = null;

            bool success = true;
            try
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TextTest.rsc.example.xml");
                reader = new StreamReader(stream, Encoding.UTF8);
                path = String.Format("{0}\\example.xml", Directory.GetCurrentDirectory());
                writer = new StreamWriter(path, false, Encoding.UTF8);

                string line;
                while ((line = reader.ReadLine()) != null)
                    writer.WriteLine(line);
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex);
                success = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (writer != null)
                    writer.Close();
            }

            if (success)
            {
                Process.Start("notepad.exe", path);
            }
        }

        /// <summary>
        /// Display the About Box for the TextTest application.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="e">The arguments for this event.</param>
        private void mniAboutApp_Click(object sender, EventArgs e)
        {
            AboutForm dlg = new AboutForm();
            dlg.ShowDialog(this);
        }


        #endregion

    }
}
