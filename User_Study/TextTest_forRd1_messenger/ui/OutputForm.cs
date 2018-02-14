using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TextTest
{
    public partial class OutputForm : Form
    {
        private OptionsFlags _options;

        /// <summary>
        /// Constructs a form capable of showing options for output.
        /// </summary>
        public OutputForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Flags corresponding to the checkbox items available as options in this dialog.
        /// </summary>
        [Flags]
        public enum OptionsFlags : byte
        {
            None = 0x00,
            MainResults = 0x01,
            OptimalAlignments = 0x02,
            CharacterErrorTable = 0x04,
            ConfusionMatrix = 0x08,
            OptimalAlignmentsWithStreams = 0x10,
            CharacterErrorTableFromStreams = 0x20,
            ConfusionMatrixFromStreams = 0x40,
            All = 0x7F
        }

        /// <summary>
        /// Gets a set of flags representing the checkbox items that were checked.
        /// </summary>
        public OptionsFlags Options
        {
            get { return _options; }
        }

        /// <summary>
        /// Gets the number of options selected after the dialog has been closed.
        /// </summary>
        public int NumSelected
        {
            get
            {
                int count = 0;
                byte flag = 0x01;
                byte allFlags = (byte) OptionsFlags.All;
                while (allFlags > 0x00)
                {
                    if (((byte) _options & flag) != 0x00)
                        count++;
                    flag <<= 1;
                    allFlags >>= 1;
                }
                return count;
            }
        }

        /// <summary>
        /// Gets the filename extension for each of the output file options available.
        /// </summary>
        /// <param name="oneOption">One option to get the extension for. Should NOT be a 
        /// bitwise composite of options for this function.</param>
        /// <returns></returns>
        public string GetExtensionFor(OptionsFlags oneOption)
        {
            string ext = String.Empty;
            switch (oneOption)
            {
                case OptionsFlags.MainResults:
                    ext = ".csv";
                    break;
                case OptionsFlags.OptimalAlignments:
                    ext = ".align.txt";
                    break;
                case OptionsFlags.CharacterErrorTable:
                    ext = ".table.csv";
                    break;
                case OptionsFlags.ConfusionMatrix:
                    ext = ".matrix.csv";
                    break;
                case OptionsFlags.OptimalAlignmentsWithStreams:
                    ext = ".salign.txt";
                    break;
                case OptionsFlags.CharacterErrorTableFromStreams:
                    ext = ".stable.csv";
                    break;
                case OptionsFlags.ConfusionMatrixFromStreams:
                    ext = ".smatrix.csv";
                    break;
                default:
                    Debug.Fail("Invalid OptionsFlags parameter in OutputForm.GetExtensionFor.");
                    break;
            }
            return ext;
        }

        /// <summary>
        /// When the form is closed, set our Options flags based on which checkboxes were selected.
        /// The caller can then check the Options property against the flags to extract the items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            byte options = 0x00;
            byte flag = 0x01;
            for (int i = 0; i < chklstOptions.Items.Count; i++)
            {
                if (chklstOptions.GetItemChecked(i))
                    options |= flag;
                flag <<= 1;
            }
            _options = (OptionsFlags) options; // copy
        }

        /// <summary>
        /// Enables the OK button when one or more items within the checkbox list are selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chklstOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdOK.Enabled = (chklstOptions.CheckedIndices.Count > 0);
        }

        /// <summary>
        /// When the "Select All" link is clicked, select all the checkboxes in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnklblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < chklstOptions.Items.Count; i++)
            {
                chklstOptions.SetItemChecked(i, true);
            }
            cmdOK.Enabled = true; // the "Select All" link does not trigger the SelectedIndexChanged 
                                  // event for chklstOptions, so manually set this here.
        }

        /// <summary>
        /// When the help button is clicked, we write out our embedded help.txt file into a local
        /// file and then open it with Notepad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdHelp_Click(object sender, EventArgs e)
        {
            string path = String.Empty;
            StreamReader reader = null;
            StreamWriter writer = null;

            bool success = true;
            try
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TextTest.rsc.help.txt");
                reader = new StreamReader(stream, Encoding.ASCII);
                path = String.Format("{0}\\help.txt", Directory.GetCurrentDirectory());
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

        
    }
}
