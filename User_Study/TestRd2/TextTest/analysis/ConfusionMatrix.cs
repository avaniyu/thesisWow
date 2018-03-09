using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TextTest
{
    public class ConfusionMatrix
    {
        #region Fields

        protected readonly Charset _charset;
        protected Dictionary<char, Dictionary<char, double>> _matrix;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a confusion matrix showing the presented, transcribed characters along with
        /// insertion and omission (deletion) errors.
        /// </summary>
        /// <remarks>The matrix is as defined by MacKenzie & Soukoreff in their 2002 NordiCHI 
        /// paper entitled, "A Character-level Error Analysis Technique for Evaluating Text 
        /// Entry Methods." http://dx.doi.org/10.1145/572020.572056 </remarks>
        public ConfusionMatrix()
        {
            _charset = new Charset();
            _charset.Add('–'); // (char) 150
            _charset.Add('¤'); // (char) 164

            // create the confusion matrix, defined as a Dictionary whose keys are 
            // the presented characters and whose values are Dictionaries whose keys 
            // are the transcribed characters and whose values are the counts.
            _matrix = new Dictionary<char, Dictionary<char, double>>(_charset.Count);
            for (int i = 0; i < _charset.Count; i++)
            {
                Dictionary<char, double> d = new Dictionary<char, double>(_charset.Count);
                for (int j = 0; j < _charset.Count; j++)
                    d.Add(_charset[j], 0.0);
                _matrix.Add(_charset[i], d);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears all the entries in the confusion matrix and resets the counts to zero for 
        /// every character.
        /// </summary>
        public virtual void Clear()
        {
            _matrix.Clear();
            for (int i = 0; i < _charset.Count; i++)
            {
                Dictionary<char, double> d = new Dictionary<char, double>(_charset.Count);
                for (int j = 0; j < _charset.Count; j++)
                    d.Add(_charset[j], 0.0);
                _matrix.Add(_charset[i], d);
            }
        }

        /// <summary>
        /// Records the confusion matrix entries evident in the aligned presented
        /// and transcribed strings. One alignment of nAlign is passed in. nAlign is 
        /// used for weighting the entries.
        /// </summary>
        /// <param name="P">An aligned presented string.</param>
        /// <param name="T">An aligned transcribed string.</param>
        /// <param name="IS">Ignored in this version. Pass null.</param>
        /// <param name="nAlign">The number of optimal alignments, for weighting.</param>
        public virtual void RecordEntries(string P, string T, string IS, int nAlign)
        {
            Debug.Assert(P.Length == T.Length, "P and T are not aligned in ConfusionMatrix.RecordConfusions.");

            for (int i = 0; i < P.Length; i++)
            {
                if (P[i] == '–')                            // insertion in T  // (char) 150
                    RecordEntry(_matrix, '–', T[i], nAlign);                   // (char) 150
                else if (T[i] == '–')                       // omission from T // (char) 150
                    RecordEntry(_matrix, P[i], '–', nAlign);                   // (char) 150
                else if (P[i] != T[i])                      // substitution
                    RecordEntry(_matrix, P[i], T[i], nAlign);
                else if (P[i] == T[i])                      // correct entry
                    RecordEntry(_matrix, P[i], T[i], nAlign);
                else
                    Debug.Fail("Unhandled case in ConfusionMatrix.RecordConfusions.");
            }
        }

        /// <summary>
        /// Records an entry in the given confusion matrix at (p, t). If either of the given characters is 
        /// outside the character set defined by the matrix, the OOCS character is used to record the entry.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="ch"></param>
        /// <param name="nAlign"></param>
        protected virtual void RecordEntry(Dictionary<char, Dictionary<char, double>> m, char p, char t, int nAlign)
        {
            Debug.Assert(m.ContainsKey('¤'), "Matrix does not contain the out-of-character-set character in ConfusionMatrix.RecordEntry.");
            foreach (KeyValuePair<char, Dictionary<char, double>> kvp in m)
                Debug.Assert(kvp.Value.ContainsKey('¤'), "Matrix does not contain the out-of-character-set character in ConfusionMatrix.RecordEntry.");

            if (m.ContainsKey(p)) // does the matrix have the presented character p?
            {
                if (m[p].ContainsKey(t)) // if so, does it have the transcribed character t?
                    m[p][t] += (1.0 / nAlign); // if so, add a tally at (p, t)
                else
                    m[p]['¤'] += (1.0 / nAlign); // if it has the presented character but not the transcribed one, add a tally at (p, '¤')
            }
            else if (m['¤'].ContainsKey(t)) // if the matrix does not contain the presented character p, does it contain the transcribed character t?
            {
                m['¤'][t] += (1.0 / nAlign); // if so, add a tally at ('¤', t)
            }
            else
            {
                m['¤']['¤'] += (1.0 / nAlign); // if not, add a tally at ('¤', '¤')
            }
        }

        #endregion

        #region File Writing

        /// <summary>
        /// Writes out a CSV file representing a confusion matrix with rows indicating the
        /// presented characters and columns representing the transcribed characters. 
        /// </summary>
        /// <param name="writer">A file writer to do the writing.</param>
        /// <returns>True; it is assumed that the encompassing writer will use try/catch/finally
        /// to catch any exceptions that may occur during the writing process.</returns>
        /// <remarks>The matrix is as defined by MacKenzie & Soukoreff in their 2002 NordiCHI 
        /// paper entitled, "A Character-level Error Analysis Technique for Evaluating Text 
        /// Entry Methods." http://dx.doi.org/10.1145/572020.572056 </remarks>
        public virtual bool WriteToCsv(StreamWriter writer)
        {
            writer.WriteLine("Credit: http://dx.doi.org/10.1145/572020.572056");
            writer.WriteLine("ROWS = presented, COLUMNS = transcribed");

            // write the top row (columns) of characters
            foreach (char t in _charset)
            {
                if (t == ' ')
                    writer.Write(",Space");
                else if (t == '–') // (char) 150
                    writer.Write(",Deletion");
                else if (t == '¤') // (char) 164
                    writer.Write(",Other");
                else
                    writer.Write(",{0}", t);
            }
            writer.WriteLine();
            
            // now write each row
            foreach (char p in _charset)
            {
                if (p == ' ')
                    writer.Write("Space");
                else if (p == '–') // (char) 150
                    writer.Write("Insertion");
                else if (p == '¤') // (char) 164
                    writer.Write("Other");
                else
                    writer.Write("{0}", p); // write the presented char

                // now write the acutal confusion counts
                foreach (char t in _charset)
                    writer.Write(",{0}", _matrix[p][t]);
                writer.WriteLine();
            }

            return true;
        }

        #endregion

    }
}