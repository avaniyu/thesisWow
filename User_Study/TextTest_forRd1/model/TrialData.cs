using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Xml;
using WobbrockLib;
using WobbrockLib.Extensions;

namespace TextTest
{
    /// <summary>
    /// Data for a single text entry trial, which is composed of time-stamped key entries.
    /// </summary>
    public class TrialData : IXmlLoggable
    {
        #region Fields

        private int _trialNo;
        private bool _testing;
        private string _presented;
        private string _transcribed;
        private List<EntryData> _entries;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty trial instance.
        /// </summary>
        public TrialData()
        {
            // do nothing
        }

        /// <summary>
        /// Constructs a trial from the given information.
        /// </summary>
        /// <param name="trialNo"></param>
        /// <param name="presented"></param>
        public TrialData(int trialNo, string presented)
        {
            _trialNo = trialNo;
            _presented = presented;
        }

        #endregion

        #region Entries

        /// <summary>
        /// Gets or sets the list of entries in the input stream for this trial. Note that
        /// this property does not affect the separately-recorded transcribed string for 
        /// this trial.
        /// </summary>
        public List<EntryData> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }

        /// <summary>
        /// Gets the number of entries in the input stream for this trial.
        /// </summary>
        public int NumEntries
        {
            get { return _entries != null ? _entries.Count : 0; }
        }

        /// <summary>
        /// Accessor to get an input stream entry for this trial at the given index.
        /// </summary>
        /// <param name="index">The index of the entry to get.</param>
        /// <returns>The entry at the given index, or null if out of bounds.</returns>
        public EntryData this[int index]
        {
            get
            {
                if (0 <= index && index < _entries.Count)
                {
                    return _entries[index];
                }
                return null;
            }
        }

        /// <summary>
        /// Adds an entry to the input stream for this trial. Has no effect on the
        /// separately-recorded transcribed string property of this trial.
        /// </summary>
        /// <param name="ed"></param>
        public void Add(EntryData ed)
        {
            if (_entries == null)
                _entries = new List<EntryData>();
            _entries.Add(ed);
        }

        /// <summary>
        /// Inserts an entry for this trial. Has no effect on the separately-recorded transcribed 
        /// string property of this trial. The index must be in bounds for this method to succeed.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ed"></param>
        public void InsertAt(int index, EntryData ed)
        {
            if (_entries != null && 0 <= index && index < _entries.Count)
            {
                _entries.Insert(index, ed);
            }
        }

        /// <summary>
        /// Removes an input stream entry for this trial. Has no effect on the separately-recorded 
        /// transcribed string property of this trial. The index must be in bounds for this method 
        /// to succeed.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public EntryData RemoveAt(int index)
        {
            EntryData ed = null;
            if (_entries != null && 0 <= index && index < _entries.Count)
            {
                ed = _entries[index];
                _entries.RemoveAt(index);
            }
            return ed;
        }

        /// <summary>
        /// Clears all of the input stream entries for this trial. Has no effect on the
        /// separately-recorded transcribed string property of this trial.
        /// </summary>
        public void Clear()
        {
            if (_entries != null)
                _entries.Clear();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the number for this trial.
        /// </summary>
        public int TrialNo
        {
            get { return _trialNo; }
            set { _trialNo = value; }
        }

        /// <summary>
        /// Gets or sets the presented string for this trial.
        /// </summary>
        public string Presented
        {
            get { return _presented; }
            set { _presented = value; }
        }

        /// <summary>
        /// Gets or sets the transcribed string for this trial. Note that this property does not
        /// affect the input stream entries that generate the transcribed string. Use VerifyStream
        /// to ensure they match.
        /// </summary>
        public string Transcribed
        {
            get { return _transcribed; }
            set { _transcribed = value; }
        }

        /// <summary>
        /// Gets or sets whether this trial was a practice trial.
        /// </summary>
        public bool IsPractice
        {
            get { return !_testing; }
            set { _testing = !value; }
        }

        /// <summary>
        /// Gets or sets whether this trial was a test trial.
        /// </summary>
        public bool IsTesting
        {
            get { return _testing; }
            set { _testing = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process the input stream to result in the transcribed string.
        /// </summary>
        /// <returns></returns>
        public string TranscribeStream()
        {
            if (_entries == null || _entries.Count == 0)
                return String.Empty;

            // make a pass backwards over the input stream, gathering up characters
            // in reverse order that should be in the final transcribed string.
            StringBuilder sb = new StringBuilder(_entries.Count);
            for (int i = _entries.Count - 1, bksp = 0; i >= 0; i--)
            {
                if (_entries[i].Char == '\b')                          
                    bksp++;
                else if (bksp == 0)
                    sb.Insert(0, _entries[i].Char); // insert at start
                else
                    bksp--;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Verify the input stream to ensure that it matches the recorded transcribed string.
        /// </summary>
        /// <returns></returns>
        public bool VerifyStream()
        {
            string ts = this.TranscribeStream();
            return (ts == _transcribed);
        }

        /// <summary>
        /// Reanders the input stream as a displayable string. Currently this just converts
        /// all backspaces '\b' to less-than signs.
        /// </summary>
        /// <remarks>
        /// Input streams are rendered using the following ASCII codes that are not found
        /// on typical keyboards to avoid collisions with text entered during studies:
        ///     
        ///     ‹ (139)     backspace
        ///     – (150)     insertion / omission
        ///     · (183)     input stream spacer     
        /// </remarks>
        public string StreamString
        {
            get
            {
                if (_entries == null || _entries.Count == 0)
                    return String.Empty;

                StringBuilder sb = new StringBuilder(_entries.Count);
                for (int i = 0; i < _entries.Count; i++)
                {
                    if (_entries[i].Char == '\b')
                        sb.Append('‹'); // (char) 139
                    else
                        sb.Append(_entries[i].Char);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Computes the optimal alignments for this trial's presented and transcribed strings.
        /// </summary>
        /// <param name="Plist">A list of aligned presented strings.</param>
        /// <param name="Tlist">A parallel list of aligned transcribed strings.</param>
        /// <returns>The number of alignment pairs.</returns>
        public int ComputeAlignments(out List<string> Plist, out List<string> Tlist)
        {
            int[,] D = new int[_presented.Length + 1, _transcribed.Length + 1];
            MSDMatrix(_presented, _transcribed, ref D);

            Plist = new List<string>();
            Tlist = new List<string>();
            Align(D, _presented.Length, _transcribed.Length, _presented, _transcribed, String.Empty, String.Empty, ref Plist, ref Tlist);

            Debug.Assert(Plist.Count == Tlist.Count, "Plist and Tlist are unequal in TrialData.ComputeAlignments().");
            return Plist.Count;
        }

        /// <summary>
        /// Computes the optimal alignments for this trial's presented and transcribed strings
        /// and input streams.
        /// </summary>
        /// <param name="Plist">A list of aligned presented strings.</param>
        /// <param name="Tlist">A parallel list of aligned transcribed strings.</param>
        /// <param name="ISlist">A parallel list of aligned input streams.</param>
        /// <returns>The number of alignment triplets.</returns>
        public int ComputeAlignments(out List<string> Plist, out List<string> Tlist, out List<string> ISlist)
        {
            int nAlign = this.ComputeAlignments(out Plist, out Tlist);
            ISlist = new List<string>(nAlign);

            for (int i = 0; i < nAlign; i++) // for each of the P, T alignments
            {
                StringBuilder P = new StringBuilder(Plist[i]);
                StringBuilder T = new StringBuilder(Tlist[i]);
                StringBuilder IS = new StringBuilder(this.StreamString);

                List<byte> bits = this.GetTranscribedStreamPositions(); // bitflag positions in IS that are in T
                Debug.Assert(bits.Count == this.StreamLength, "Bitflag count and input stream length do not match in TrialData.ComputeAlignments.");

                for (int j = 0; j < Math.Max(bits.Count, T.Length); j++)
                {
                    if (j < T.Length && T[j] == '–') // insertion or omission // (char) 150
                    {
                        IS.Insert(j, '·');      // spacer in IS // (char) 183
                        bits.Insert(j, 0x00);   // keep flags in sync with IS
                    }
                    else if (j < bits.Count && bits[j] == 0x00) // input stream char not in T
                    {
                        P.Insert(j, '·');       // spacer in P // (char) 183
                        T.Insert(j, '·');       // spacer in T // (char) 183
                    }
                }
                Plist[i] = P.ToString();        // replace
                Tlist[i] = T.ToString();        // replace
                ISlist.Add(IS.ToString());      // add
            }

            return nAlign;
        }

        /// <summary>
        /// Gets the number of optimal alignments between presented and transcribed strings.
        /// </summary>
        public int NumAlignments
        {
            get
            {
                List<string> Plist, Tlist;
                return ComputeAlignments(out Plist, out Tlist);
            }
        }

        #endregion

        #region Measures

        /// <summary>
        ///  Gets the duration of this trial, in seconds.
        /// </summary>
        public double Duration
        {
            get
            {
                return (_entries != null && _entries.Count > 0) ? TimeEx.Ticks2Sec(_entries[_entries.Count - 1].Ticks - _entries[0].Ticks) : 0.0;
            }
        }

        /// <summary>
        /// Gets the standard words per minute measure. A word is defined as five characters, 
        /// including spaces. Since time is measured from the first character entered, we must 
        /// substract one from the length so as not to count the first character. See
		/// http://www.yorku.ca/mack/RN-TextEntrySpeed.html
        /// </summary>
        public double WPM
        {
            get
            {
                double seconds = this.Duration;
                if (seconds == 0.0)
                    return 0.0;

                if (_transcribed.Length == 0)
                    return 0.0;
                else
                    return ((_transcribed.Length - 1) / seconds) * 60.0 * 0.2;
            }
        }

        /// <summary>
        /// Gest the adjusted words per minute speed, which is WPM multiplied by one minus the
        /// uncorrected error rate, since uncorrected errors are at odds with WPM. If there are
        /// no uncorrected errors, then AdjWPM will equal WPM.
        /// </summary>
        public double AdjWPM
        {
            get
            {
                double seconds = this.Duration;
                if (seconds == 0.0)
                    return 0.0;

                double wpm = this.WPM;
                double unc = this.UncorrectedErrorRate;
                return (wpm * (1.0 - unc));
            }
        }

        /// <summary>
        /// Gets characters per second. Equivalent to words per minute.
        /// </summary>
        public double CPS
        {
            get
            {
                double seconds = this.Duration;
                if (seconds == 0.0)
                    return 0.0;

                if (_transcribed.Length == 0)
                    return 0.0;
                else
                    return ((_transcribed.Length - 1) / seconds);
            }
        }

        /// <summary>
        /// Gets keystrokes per second. This measure cares nothing for the length of the final transcription, 
        /// as WPM and CPS must. This is a measure of a raw data transfer rate, used for computing upper bounds 
        /// on human performance (i.e., assume all characters are correct). The stream length itself is used, so 
		/// backspaces count as much as all other characters.
        /// </summary>
        public double KSPS
        {
            get
            {
                double seconds = this.Duration;
                if (seconds == 0.0)
                    return 0.0;

                return (_entries.Count - 1) / seconds;
            }
        }

        /// <summary>
        ///  Gets the Levenshtein minimum string distance between the presented string and transcribed string 
        ///  for this trial.
        /// </summary>
        public int MSD
        {
            get
            {
                int[,] D = new int[_presented.Length + 1, _transcribed.Length + 1];
                return MSDMatrix(_presented, _transcribed, ref D);
            }
        }

        /// <summary>
        /// Gets the keystrokes per character measure, which is the ratio input stream length to the final 
        /// transcribed string length. For an input stream with no corrections (e.g., backspaces), this ratio 
        /// should be 1.0.
        /// </summary>
        public double KSPC
        {
            get
            {
                return (_entries != null && _transcribed.Length > 0) ? _entries.Count / (double) _transcribed.Length : 0.0;
            }
        }

        /// <summary>
        ///  Gets the uncorrected error rate measure from Soukoreff & MacKenzie (CHI 2003).
        /// </summary>
        public double UncorrectedErrorRate
        {
            get
            {
                int c = this.Correct;
                int inf = this.IncorrectNotFixed;
                int ifx = this.IncorrectFixed;
                return (c + inf + ifx > 0) ? inf / (double) (c + inf + ifx) : 0.0;
            }
        }

        /// <summary>
        /// Gets the corrected error rate measure from Soukoreff & MacKenzie (CHI 2003).
        /// </summary>
        public double CorrectedErrorRate
        {
            get
            {
                int c = this.Correct;
                int inf = this.IncorrectNotFixed;
                int ifx = this.IncorrectFixed;
                return (c + inf + ifx > 0) ? ifx / (double) (c + inf + ifx) : 0.0;
            }
        }

        /// <summary>
        ///  Gets the total error rate measure from Soukoreff & MacKenzie (CHI 2003).
        /// </summary>
        public double TotalErrorRate
        {
            get
            {
                int c = this.Correct;
                int inf = this.IncorrectNotFixed;
                int ifx = this.IncorrectFixed;
                return (c + inf + ifx > 0) ? (inf + ifx) / (double) (c + inf + ifx) : 0.0;
            }
        }

        /// <summary>
        ///  Gets the correction efficiency measure from Soukoreff & MacKenzie (CHI 2003).
        /// </summary>
        public double CorrectionEfficiency
        {
            get
            {
                int ifx = this.IncorrectFixed;
                int fix = this.Fixes;
                return (fix > 0) ? ifx / (double) fix : 1.0;
            }
        }

        /// <summary>
        ///  Gets the participant conscientiousness measure from Soukoreff & MacKenzie (CHI 2003).
        /// </summary>
        public double ParticipantConscientiousness
        {
            get
            {
                int ifx = this.IncorrectFixed;
                int inf = this.IncorrectNotFixed;
                return (ifx + inf > 0) ? ifx / (double) (ifx + inf) : 1.0;
            }
        }

        /// <summary>
        ///  Gets the utilized bandwidth measure from Soukoreff & MacKenzie (CHI 2003).
        /// </summary>
        public double UtilizedBandwidth
        {
            get
            {
                int c = this.Correct;
                int inf = this.IncorrectNotFixed;
                int ifx = this.IncorrectFixed;
                int fix = this.Fixes;
                return (c + inf + ifx + fix > 0) ? c / (double) (c + inf + ifx + fix) : 1.0;
            }
        }

        /// <summary>
        /// Gets the wasted bandwidth measure from Soukoreff & MacKenzie (CHI 2003).
        /// </summary>
        public double WastedBandwidth
        {
            get
            {
                int c = this.Correct;
                int inf = this.IncorrectNotFixed;
                int ifx = this.IncorrectFixed;
                int fix = this.Fixes;
                return (c + inf + ifx + fix > 0) ? (inf + ifx + fix) / (double) (c + inf + ifx + fix) : 0.0;
            }
        }

        /// <summary>
        ///  Gets the number of characters in the "C" (Correct) character class from Soukoreff & MacKenzie (CHI 2003).
        /// </summary>
        public int Correct
        {
            get
            {
                return Math.Max(_presented.Length, _transcribed.Length) - this.MSD;
            }
        }

        /// <summary>
        /// Gets the number of characters in the "INF" (Incorret Not Fixed) character class from Soukoreff & 
        /// MacKenzie (CHI 2003).
        /// </summary>
        public int IncorrectNotFixed
        {
            get { return this.MSD; }
        }

        /// <summary>
        /// Gets the number of characters in the "IF" (Incorrect Fixed) character class from Soukoreff & MacKenzie (CHI 2003).
        /// </summary>
        public int IncorrectFixed
        {
            get
            {
                return (this.StreamLength - this.TranscribedLength - this.Fixes);
            }
        }

        /// <summary>
        /// Gets the number of characters in the "F" (Fixes) character class from Soukoreff & MacKenzie (CHI 2003). The only
        /// character currently regarded as a "fix" is the backspace character.
        /// </summary>
        public int Fixes
        {
            get
            {
                int fixes = 0;
                if (_entries != null)
                {
                    for (int i = 0; i < _entries.Count; i++)
                    {
                        if (_entries[i].Char == '\b')
                            fixes++;
                    }
                }
                return fixes;
            }
        }

        /// <summary>
        /// Gets the length of the presented string.
        /// </summary>
        public int PresentedLength
        {
            get { return _presented.Length; }
        }

        /// <summary>
        /// Gets the length of the transcribed string.
        /// </summary>
        public int TranscribedLength
        {
            get { return _transcribed.Length; }
        }

        /// <summary>
        /// Gets the length of the input stream, which includes backspaces and backspaced characters.
        /// </summary>
        public int StreamLength
        {
            get { return (_entries != null) ? _entries.Count : 0; }
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Computes the minimum string distance matrix that yields the minimum string distance value itself
        /// and also is used in computing the optimal alignments between P and T.
        /// </summary>
        /// <param name="P">The presented string.</param>
        /// <param name="T">The transcribed string.</param>
        /// <param name="D">The 2-D MSD matrix to be filled in by reference. It must be initialized with 
        /// dimensions [P.Length + 1, T.Length + 1].</param>
        /// <returns>The minimum string distance.</returns>
        private int MSDMatrix(string P, string T, ref int[,] D)
        {
            for (int i = 0; i <= P.Length; i++)
                D[i, 0] = i;

            for (int j = 0; j <= T.Length; j++)
                D[0, j] = j;

            for (int i = 1; i <= P.Length; i++)
                for (int j = 1; j <= T.Length; j++)
                    D[i, j] = (int) StatsEx.Min3(
                        D[i - 1, j] + 1, 
                        D[i, j - 1] + 1, 
                        D[i - 1, j - 1] + ((P[i - 1] == T[j - 1]) ? 0 : 1)
                        );

            return D[P.Length, T.Length];   // minimum string distance
        }

        /// <summary>
        /// Aligns P and T. The aligned strings are returned by reference in two parallel lists.
        /// </summary>
        /// <param name="D">The 2-D MSD matrix. It must be initialized with dimensions [P.Length + 1, T.Length + 1].</param>
        /// <param name="x">A position value for indexing into D and P.</param>
        /// <param name="y">A position value for indexing into D and T.</param>
        /// <param name="P">The original presented string.</param>
        /// <param name="T">The original transcribed string.</param>
        /// <param name="aP">The will-be-aligned presented string.</param>
        /// <param name="aT">The will-be-aligned transcribed string.</param>
        /// <param name="Plist">A list of aligned presented strings.</param>
        /// <param name="Tlist">A parallel list of aligned transcribed strings.</param>
        private void Align(int[,] D, int x, int y, string P, string T, string aP, string aT, ref List<string> Plist, ref List<string> Tlist)
        {
            if (x == 0 && y == 0)
            {
                Plist.Add(aP);
                Tlist.Add(aT);
                return;
            }
            if (x > 0 && y > 0)
            {
                if (D[x, y] == D[x - 1, y - 1] && P[x - 1] == T[y - 1])
                    Align(D, x - 1, y - 1, P, T, P[x - 1] + aP, T[y - 1] + aT, ref Plist, ref Tlist);
                if (D[x, y] == D[x - 1, y - 1] + 1)
                    Align(D, x - 1, y - 1, P, T, P[x - 1] + aP, T[y - 1] + aT, ref Plist, ref Tlist);
            }
            if (x > 0 && D[x, y] == D[x - 1, y] + 1)
                Align(D, x - 1, y, P, T, P[x - 1] + aP, "–" + aT, ref Plist, ref Tlist); // omission from T (add "–" to T) // (char) 150

            if (y > 0 && D[x, y] == D[x, y - 1] + 1)
                Align(D, x, y - 1, P, T, "–" + aP, T[y - 1] + aT, ref Plist, ref Tlist); // insertion in T (add "–" to P) // (char) 150
        }

        /// <summary>
        /// Gets a list of bytes corresponding to the length of the input stream,
        /// where bits[i] is "on" iff _entries[i] is in the transcribed string.
        /// </summary>
        /// <returns>A list of bytes as long as the input stream, where an "on" byte
        /// value of 0x01 indicates an input stream position whose character is in
        /// the transcribed string. An "off" byte value of 0x00 indicates that input
        /// stream character is not in the transcribed string.</returns>
        private List<byte> GetTranscribedStreamPositions()
        {
            List<byte> bits = new List<byte>();
            if (_entries == null || _entries.Count == 0)
                return bits;

            bits.Capacity = _entries.Count;
            for (int i = 0; i < _entries.Count; i++)
                bits.Add(0x00); // set all to zero initially

            for (int i = _entries.Count - 1, bksp = 0; i >= 0; i--)
            {
                if (_entries[i].Char == '\b')
                    bksp++;
                else if (bksp == 0)
                    bits[i] = 0x01;
                else
                    bksp--;
            }
            return bits;
        }

        #endregion

        #region IXmlLoggable

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool WriteXmlHeader(XmlTextWriter writer)
        {
            writer.WriteStartElement("Trial");
            writer.WriteAttributeString("number", XmlConvert.ToString(this.TrialNo));
            writer.WriteAttributeString("testing", XmlConvert.ToString(this.IsTesting));
            writer.WriteAttributeString("entries", XmlConvert.ToString(this.NumEntries));

            writer.WriteStartElement("Presented");
            writer.WriteString(this.Presented);
            writer.WriteEndElement(); // </Presented>

            // write out the individual character entries
            for (int i = 0; i < this.NumEntries; i++)
            { 
                _entries[i].WriteXmlHeader(writer);
                _entries[i].WriteXmlFooter(writer);
            }

            writer.WriteStartElement("Transcribed");
            writer.WriteString(this.Transcribed);
            writer.WriteEndElement(); // </Transcribed>

            writer.WriteEndElement(); // </Trial>

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool WriteXmlFooter(XmlTextWriter writer)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public bool ReadFromXml(XmlTextReader reader)
        {
            reader.Read(); // <Trial>
            if (reader.Name != "Trial")
                throw new XmlException("XML format error: Expected the <Trial> element.");

            _trialNo = XmlConvert.ToInt32(reader.GetAttribute("number"));
            _testing = XmlConvert.ToBoolean(reader.GetAttribute("testing"));
            int entries = XmlConvert.ToInt32(reader.GetAttribute("entries"));

            reader.Read(); // <Presented>
            if (reader.Name != "Presented")
                throw new XmlException("XML format error: Expected the <Presented> element.");
            _presented = reader.ReadString(); // reads the string and advances to the end element
            if (reader.Name != "Presented" || reader.NodeType != XmlNodeType.EndElement)
                throw new XmlException("XML format error: Expected the </Presented> element.");

            // read in all the character entries
            _entries = new List<EntryData>(entries);
            int backspaces = 0;
            for (int i = 0; i < entries; i++)
            {
                EntryData ed = new EntryData();
                if (ed.ReadFromXml(reader))
                {
                    _entries.Add(ed);
                    if (ed.Code == '\b')
                        backspaces++;
                }
            }

            reader.Read(); // <Transcribed>
            if (reader.Name != "Transcribed")
                throw new XmlException("XML format error: Expected the <Transcribed> element.");
            _transcribed = reader.ReadString(); // reads the string and advances to the end element
            if ((entries - backspaces * 2 > 0) && (reader.Name != "Transcribed" || reader.NodeType != XmlNodeType.EndElement))
                throw new XmlException("XML format error: Expected the </Transcribed> element.");

            reader.Read(); // </Trial>
            if (reader.Name != "Trial" || reader.NodeType != XmlNodeType.EndElement)
                throw new XmlException("XML format error: Expected the </Trial> element.");

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool WriteResultsToTxt(StreamWriter writer)
        {
            writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22}",
                this.TrialNo,                       // Trial
                this.IsTesting ? 1 : 0,             // Testing?
                this.Duration,                      // Seconds
                this.WPM,                           // WPM
                this.AdjWPM,                        // AdjWPM
                this.CPS,                           // CPS
                this.KSPS,                          // KSPS
                this.MSD,                           // MSD
                this.KSPC,                          // KSPC
                this.UncorrectedErrorRate,          // UncErrRate
                this.CorrectedErrorRate,            // CorErrRate
                this.TotalErrorRate,                // TotErrRate
                this.CorrectionEfficiency,          // Effic
                this.ParticipantConscientiousness,  // Consc
                this.UtilizedBandwidth,             // UtilBand
                this.WastedBandwidth,               // WasteBand
                this.Correct,                       // |C|
                this.IncorrectNotFixed,             // |INF|
                this.IncorrectFixed,                // |IF|
                this.Fixes,                         // |F|
                this.PresentedLength,               // |P|
                this.TranscribedLength,             // |T|
                this.StreamLength                   // |IS|
                );

            return true;
        }

        #endregion

        #region Output Files

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="useInputStream"></param>
        /// <returns></returns>
        public bool WriteOptimalAlignments(StreamWriter writer, bool useInputStream)
        {
            int nAlign = 0;
            List<string> Plist, Tlist;

            if (!useInputStream) // P, T
            {
                nAlign = ComputeAlignments(out Plist, out Tlist);
                for (int i = 0; i < nAlign; i++)
                {
                    writer.WriteLine("P{0}.{1}: {2}", this.TrialNo, i + 1, Plist[i]);
                    writer.WriteLine("T{0}.{1}: {2}", this.TrialNo, i + 1, Tlist[i]);
                }
            }
            else // P, T, IS
            {
                List<string> ISlist;
                nAlign = ComputeAlignments(out Plist, out Tlist, out ISlist);
                for (int i = 0; i < nAlign; i++)
                {
                    writer.WriteLine(" P{0}.{1}: {2}", this.TrialNo, i + 1, Plist[i]);
                    writer.WriteLine(" T{0}.{1}: {2}", this.TrialNo, i + 1, Tlist[i]);
                    writer.WriteLine("IS{0}.{1}: {2}", this.TrialNo, i + 1, ISlist[i]);
                }
            }
            writer.WriteLine();

            return true;
        }

        #endregion
    }
}
