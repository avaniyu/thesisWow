using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;    
using System.Xml;
using WobbrockLib;
using WobbrockLib.Extensions;

namespace TextTest
{
    /// <summary>
    /// Data for a tex entry test session, which is composed of text entry trials.
    /// </summary>
    public class SessionData : IXmlLoggable
    {
        #region Fields

        private long _ticks; // when the test was started
        private List<TrialData> _trials; // trials for this session

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty session instance.
        /// </summary>
        public SessionData()
        {
            // do nothing
        }

        /// <summary>
        /// Constructs a session from the given information.
        /// </summary>
        /// <param name="ticks"></param>
        public SessionData(long ticks)
        {
            _ticks = ticks;
        }

        #endregion

        #region Trials

        /// <summary>
        /// Gets or sets the list of trials for this session.
        /// </summary>
        public List<TrialData> Trials
        {
            get { return _trials; }
            set { _trials = value; }
        }

        /// <summary>
        /// Gets the number of trials for this session.
        /// </summary>
        public int NumTrials
        {
            get { return _trials != null ? _trials.Count : 0; }
        }

        /// <summary>
        /// Accessor to get an individual trial at the given index.
        /// </summary>
        /// <param name="index">The index of the entry to get.</param>
        /// <returns>The trial at the given index, or null if out of bounds.</returns>
        public TrialData this[int index]
        {
            get
            {
                if (0 <= index && index < _trials.Count)
                {
                    return _trials[index];
                }
                return null;
            }
        }

        /// <summary>
        /// Adds a trial to this session.
        /// </summary>
        /// <param name="td"></param>
        public void Add(TrialData td)
        {
            if (_trials == null)
                _trials = new List<TrialData>();
            _trials.Add(td);
        }

        /// <summary>
        /// Inserts a trial within this session. The index must be within bounds for this
        /// method to succeed.
        /// </summary>
        /// <param name="index"></param>
        public void InsertAt(int index, TrialData td)
        {
            if (_trials != null && 0 <= index && index < _trials.Count)
            {
                _trials.Insert(index, td);
            }
        }

        /// <summary>
        /// Removes the trial from this session. The index must be in bounds for this 
        /// method to succeed.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TrialData RemoveAt(int index)
        {
            TrialData td = null;
            if (_trials != null && 0 <= index && index < _trials.Count)
            {
                td = _trials[index];
                _trials.RemoveAt(index);
            }
            return td;
        }

        /// <summary>
        /// Clears all trials from this session.
        /// </summary>
        public void Clear()
        {
            if (_trials != null)
                _trials.Clear();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the starting time for this session, in ticks. Note that the starting
        /// time is irrelevant to the text entry calculations, which are based on first and
        /// last character entries within the input stream.
        /// </summary>
        public long Ticks
        {
            get { return _ticks; }
            set { _ticks = value; }
        }

        /// <summary>
        /// Gets the number of practice trials within this session.
        /// </summary>
        public int NumPractice
        {
            get
            {
                int nPractice = 0;
                if (_trials != null)
                {
                    for (int i = 0; i < _trials.Count; i++)
                    {
                        if (!_trials[i].IsTesting)
                            nPractice++;
                    }
                }
                return nPractice;
            }
        }

        /// <summary>
        /// Gets the number of test trials within this session.
        /// </summary>
        public int NumTesting
        {
            get
            {
                int nTesting = 0;
                if (_trials != null)
                {
                    for (int i = 0; i < _trials.Count; i++)
                    {
                        if (_trials[i].IsTesting)
                            nTesting++;
                    }
                }
                return nTesting;
            }
        }

        #endregion

        #region IXmlLoggable

        /// <summary>
        /// Writes this session object out as an XML log, which can be read in at a later time to
        /// "re-inflate" this session object.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns>True if successful; false otherwise.</returns>
        public bool WriteXmlHeader(XmlTextWriter writer)
        {
            bool success = true;
            try
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument(true);
                writer.WriteStartElement("TextTest");
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                writer.WriteAttributeString("version", String.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build));
                writer.WriteAttributeString("trials", XmlConvert.ToString(this.NumTrials));
                writer.WriteAttributeString("ticks", XmlConvert.ToString(this.Ticks));
                writer.WriteAttributeString("seconds", XmlConvert.ToString(TimeEx.Ticks2Sec(this.Ticks, 2)));
                DateTime dt = new DateTime(this.Ticks);
                writer.WriteAttributeString("date", String.Format("{0} {1}", dt.ToLongDateString(), dt.ToLongTimeString()));

                // write out the individual trials
                for (int i = 0; i < this.NumTrials; i++)
                {
                    _trials[i].WriteXmlHeader(writer);
                    _trials[i].WriteXmlFooter(writer);
                }

                writer.WriteEndDocument(); // </TextTest>
            }
            catch (XmlException xex)
            {
                Console.WriteLine(xex);
                success = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            return success;
        }

        /// <summary>
        /// Writes the footer, i.e., the end XML, for this session object. Often a no-op,
        /// as in this case.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns>True if successful; false otherwise.</returns>
        public bool WriteXmlFooter(XmlTextWriter writer)
        {
            bool success = true;
            try
            {
                // do nothing
            }
            catch (XmlException xex)
            {
                Console.WriteLine(xex);
                success = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                success = false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
            return success;
        }

        /// <summary>
        /// Reads in this session object from XML thereby "inflates" this session object.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>True if successful; false otherwise.</returns>
        public bool ReadFromXml(XmlTextReader reader)
        {
            bool success = true;
            try
            {
                reader.WhitespaceHandling = WhitespaceHandling.None;
                if (!reader.IsStartElement("TextTest")) // moves to content and tests the topmost element
                    throw new XmlException("XML format error: Expected the <TextTest> element.");

                int trials = XmlConvert.ToInt32(reader.GetAttribute("trials"));
                _trials = new List<TrialData>(trials);
                _ticks = XmlConvert.ToInt64(reader.GetAttribute("ticks"));

                for (int i = 0; i < trials; i++)
                {
                    TrialData td = new TrialData();
                    if (!td.ReadFromXml(reader))
                        throw new XmlException("XML data error: Failed to read in TrialData instance.");
                    else
                        _trials.Add(td);
                }

                if (trials > 0)
                {
                    reader.Read(); // </TextTest>
                    if (reader.Name != "TextTest" || reader.NodeType != XmlNodeType.EndElement)
                        throw new XmlException("XML format error: Expected the </TextTest> element.");
                }
            }
            catch (XmlException xex)
            {
                Console.WriteLine(xex);
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
            }
            return success;
        }

        /// <summary>
        /// Writes a CSV file representing the main results for each trial in this session. The results
        /// can be analyzed with customary statistical packages like R, JMP, or SPSS.
        /// </summary>
        /// <param name="writer">The stream writer to write the file.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public bool WriteResultsToTxt(StreamWriter writer)
        {
            bool success = true;
            try
            {
                writer.WriteLine("Credit: http://books.google.com/books?id=XWSc3b_gkX8C&pg=PA47&hl=en");

                writer.WriteLine("Trial,Testing?,Seconds,WPM,AdjWPM,CPS,KSPS,MSD,KSPC,UncErrRate,CorErrRate,TotErrRate,Effic,Consc,UtilBand,WasteBand,|C|,|INF|,|IF|,|F|,|P|,|T|,|IS|");

                for (int i = 0; i < _trials.Count; i++)
                {
                    TrialData td = _trials[i];
                    td.WriteResultsToTxt(writer); // have each trial write themselves, assuming the column names above
                }
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
                if (writer != null)
                    writer.Close();
            }
            return success;
        }

        #endregion

        #region Other Output Files

        /// <summary>
        /// Writes a TXT file showing the optimal alignments for each trial in this session.
        /// Optimal alignments show the possible errors that separate the presented string
        /// from the transcribed string.
        /// </summary>
        /// <param name="writer">The stream writer to write the file.</param>
        /// <param name="useInputStream">A boolean indicating whether the input stream should
        /// be considered or only the presented and transcribed strings.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public bool WriteOptimalAlignments(StreamWriter writer, bool useInputStream)
        {
            bool success = true;
            try
            {
                writer.WriteLine("{0}{1}", !useInputStream ? "Credit: http://dx.doi.org/10.1145/572020.572056" : "Credit: http://dx.doi.org/10.1145/1188816.1188819", Environment.NewLine);

                for (int i = 0; i < _trials.Count; i++)
                {
                    TrialData td = _trials[i];
                    td.WriteOptimalAlignments(writer, useInputStream);
                }
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
                if (writer != null)
                    writer.Close();
            }
            return success;
        }

        /// <summary>
        /// Writes a CSV file representing a character-level error table for this session.
        /// </summary>
        /// <param name="writer">The stream writer to write the file.</param>
        /// <param name="useInputStream">A boolean indicating whether the input stream should
        /// be considered or only the presented and transcribed strings.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public bool WriteCharacterErrorTable(StreamWriter writer, bool useInputStream)
        {
            bool success = true;
            try
            {
                CharacterErrorTable table = !useInputStream ? new CharacterErrorTable() : new CharacterErrorTableEx(); // polymorphism!

                for (int i = 0; i < _trials.Count; i++)
                {
                    TrialData td = _trials[i];
                    List<string> Plist, Tlist;
                    if (!useInputStream)
                    {
                        int nAlign = td.ComputeAlignments(out Plist, out Tlist);
                        for (int j = 0; j < nAlign; j++)
                            table.RecordEntries(Plist[j], Tlist[j], null, nAlign);
                    }
                    else
                    {
                        List<string> ISlist;
                        int nAlign = td.ComputeAlignments(out Plist, out Tlist, out ISlist);
                        for (int j = 0; j < nAlign; j++)
                            table.RecordEntries(Plist[j], Tlist[j], ISlist[j], nAlign);
                    }
                }
                table.CalculateErrorProbabilities();
                table.WriteToCsv(writer, false);
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
                if (writer != null)
                    writer.Close();
            }
            return success;
        }

        /// <summary>
        /// Writes a CSV file representing a character-level confusion matrix for this session.
        /// </summary>
        /// <param name="writer">The stream writer to write the file.</param>
        /// <param name="useInputStream">A boolean indicating whether the input stream should
        /// be considered or only the presented and transcribed strings.</param>
        /// <returns>True if successful; false otherwise.</returns>
        public bool WriteConfusionMatrix(StreamWriter writer, bool useInputStream)
        {
            bool success = true;
            try
            {
                ConfusionMatrix matrix = !useInputStream ? new ConfusionMatrix() : new ConfusionMatrixEx(); // polymorphism!

                for (int i = 0; i < _trials.Count; i++)
                {
                    TrialData td = _trials[i];
                    List<string> Plist, Tlist;
                    if (!useInputStream)
                    {
                        int nAlign = td.ComputeAlignments(out Plist, out Tlist);
                        for (int j = 0; j < nAlign; j++)
                            matrix.RecordEntries(Plist[j], Tlist[j], null, nAlign);
                    }
                    else
                    {
                        List<string> ISlist;
                        int nAlign = td.ComputeAlignments(out Plist, out Tlist, out ISlist);
                        for (int j = 0; j < nAlign; j++)
                            matrix.RecordEntries(Plist[j], Tlist[j], ISlist[j], nAlign);
                    }
                }
                matrix.WriteToCsv(writer);
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
                if (writer != null)
                    writer.Close();
            }
            return success;
        }

        #endregion
    }
}
