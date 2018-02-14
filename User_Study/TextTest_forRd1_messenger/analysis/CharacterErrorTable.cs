using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TextTest
{
    public class CharacterErrorTable
    {
        #region Fields

        protected readonly Charset _charset;
        protected Dictionary<char, double> _presentedChars;
        protected Dictionary<char, double> _uncInsertions;
        protected Dictionary<char, double> _uncSubstitutions;
        protected Dictionary<char, double> _uncOmissions;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a character-level error table that shows insertion, substitution,
        /// and deletion errors between presented and transcribed strings.
        /// </summary>
        /// <remarks>The table is as defined by MacKenzie & Soukoreff in their 2002 NordiCHI 
        /// paper entitled, "A Character-level Error Analysis Technique for Evaluating Text 
        /// Entry Methods." http://dx.doi.org/10.1145/572020.572056 </remarks>
        public CharacterErrorTable()
        {
            // add the insertion/omission character to the default alphanumeric character set
            _charset = new Charset();
            _charset.Add('–'); // (char) 150
            _charset.Add('¤'); // (char) 164 OOCS

            _presentedChars = new Dictionary<char, double>(_charset.Count);
            _uncInsertions = new Dictionary<char, double>(_charset.Count);
            _uncSubstitutions = new Dictionary<char, double>(_charset.Count);
            _uncOmissions = new Dictionary<char, double>(_charset.Count);

            for (int i = 0; i < _charset.Count; i++)
            {
                _presentedChars.Add(_charset[i], 0.0);
                _uncInsertions.Add(_charset[i], 0.0);
                _uncSubstitutions.Add(_charset[i], 0.0);
                _uncOmissions.Add(_charset[i], 0.0);
            }
        }

        /// <summary>
        /// Clears all character counts and resets them for each character to zero.
        /// </summary>
        protected virtual void Clear()
        {
            _presentedChars.Clear();
            _uncInsertions.Clear();
            _uncSubstitutions.Clear();
            _uncOmissions.Clear();

            for (int i = 0; i < _charset.Count; i++)
            {
                _presentedChars.Add(_charset[i], 0.0);
                _uncInsertions.Add(_charset[i], 0.0);
                _uncSubstitutions.Add(_charset[i], 0.0);
                _uncOmissions.Add(_charset[i], 0.0);
            }
        }

        #endregion

        #region Record Entries

        /// <summary>
        /// Records the characters occurring and the errors made in the aligned presented
        /// and transcribed strings. One alignment of "nAlign" is passed in. "nAlign" is used
        /// for weighting the errors.
        /// </summary>
        /// <param name="P">An aligned presented string.</param>
        /// <param name="T">An aligned transcribed string.</param>
        /// <param name="IS">Ignored in this version. Pass null.</param>
        /// <param name="nAlign">The number of optimal alignments, for weighting.</param>
        public virtual void RecordEntries(string P, string T, string IS, int nAlign)
        {
            Debug.Assert(P.Length == T.Length, "P and T are not aligned in CharacterErrorTable.RecordCharacters.");

            for (int i = 0; i < P.Length; i++)
            {
                // first do the weighted counting of the presented characters
                RecordEntry(_presentedChars, P[i], nAlign);

                // then do the weighted tallying of the errors
                if (P[i] == '–')                        // insertion in T   // (char) 150
                    RecordEntry(_uncInsertions, '–', nAlign);               // (char) 150
                else if (T[i] == '–')                   // omission from T  // (char) 150
                    RecordEntry(_uncOmissions, P[i], nAlign);
                else if (P[i] != T[i])                  // substitution
                    RecordEntry(_uncSubstitutions, P[i], nAlign);
            }
        }

        /// <summary>
        /// Records an entry in one of the dictionaries that count errors. If the given character is outside
        /// the character set defined by the dictionary, the OOCS character is used to record the entry.
        /// </summary>
        /// <param name="d">The given dictionary.</param>
        /// <param name="ch">The given character.</param>
        /// <param name="weight">The number of alignments, for weighting.</param>
        protected virtual void RecordEntry(Dictionary<char, double> d, char ch, int nAlign)
        {
            Debug.Assert(d.ContainsKey('¤'), "Dictionary does not contain the out-of-character-set character in CharacterErrorTable.RecordEntry.");

            if (d.ContainsKey(ch))
                d[ch] += (1.0 / nAlign);
            else
                d['¤'] += (1.0 / nAlign); // (char) 164
        }

        /// <summary>
        /// Normalizes the error probabilities recorded by the count of occurrences of
        /// each respective character. If this step is not taken, then the RecordErrors 
        /// function just keeps tallying up errors regardless of how many separate trials 
        /// were run. This function should be called immediatley after all trials in a 
        /// session are processed by RecordErrors.
        /// </summary>
        public virtual void CalculateErrorProbabilities()
        {
            foreach (char ch in _charset)
            {
                double count = _presentedChars[ch];
                if (count > 0.0)
                {
                    _uncSubstitutions[ch] /= count;
                    _uncOmissions[ch] /= count;
                    _uncInsertions[ch] /= count;
                }
            }
        }

        #endregion

        #region Character-Level Metrics

        /// <summary>
        /// Gets the total number of characters seen in the processed alignments.
        /// </summary>
        protected double TotalChars
        {
            get
            {
                double total = 0.0;
                foreach (char ch in _charset)
                    total += _presentedChars[ch];
                return total;
            }
        }

        /// <summary>
        /// Gets the total number of insertions, weighted by counts, seen in the processed
        /// alignments.
        /// </summary>
        protected double TotalInsertions
        {
            get
            {
                double total = 0.0;
                foreach (char ch in _charset)
                    total += _uncInsertions[ch] * _presentedChars[ch];
                return total;
            }
        }

        /// <summary>
        /// Gets the total number of substitutions, weighted by counts, seen in the processed
        /// alignments.
        /// </summary>
        protected double TotalSubstitutions
        {
            get
            {
                double total = 0.0;
                foreach (char ch in _charset)
                    total += _uncSubstitutions[ch] * _presentedChars[ch];
                return total;
            }
        }

        /// <summary>
        /// Gets the total number of deletions, weighted by counts, seen in the processed 
        /// alignments.
        /// </summary>
        protected double TotalOmissions
        {
            get
            {
                double total = 0.0;
                foreach (char ch in _charset)
                    total += _uncOmissions[ch] * _presentedChars[ch];
                return total;
            }
        }

        #endregion

        #region File Writing

        /// <summary>
        /// Writes the character-level error table as defined by MacKenzie & Soukoreff in
        /// their 2002 NordiCHI paper entitled, "A Character-level Error Analysis Technique 
        /// for Evaluating Text Entry Methods." http://dx.doi.org/10.1145/572020.572056
        /// </summary>
        /// <param name="writer">A file writer to do the writing.</param>
        /// <param name="wantWholeCharset">If true, a table entry for every character in the
        /// possible presented-string character set is shown; if false, then only rows whose
        /// characters actually appeared in any of the presented strings are shown.</param>
        /// <returns>True; it is assumed that the encompassing writer will use try/catch/finally
        /// to catch any exceptions that may occur during the writing process.</returns>
        public virtual bool WriteToCsv(StreamWriter writer, bool wantWholeCharset)
        {
            writer.WriteLine("Credit: http://dx.doi.org/10.1145/572020.572056");
            writer.WriteLine("Character,Count,Ins,Sub,Del,Total");
            foreach (char ch in _charset)
            {
                if (wantWholeCharset || _presentedChars[ch] > 0.0)
                {
                    double ins = _uncInsertions[ch];
                    double sub = _uncSubstitutions[ch];
                    double del = _uncOmissions[ch];
                    writer.WriteLine("{0},{1},{2},{3},{4},{5}",
                        CharToDisplay(ch),
                        _presentedChars[ch],
                        ins,
                        sub,
                        del,
                        ins + sub + del
                        );
                }
            }

            double totalCount = this.TotalChars;
            double totalIns = this.TotalInsertions;
            double totalSub = this.TotalSubstitutions;
            double totalDel = this.TotalOmissions;

            // write the total line
            writer.WriteLine("Total,{0},{1},{2},{3},{4}",
                totalCount,
                totalIns,
                totalSub,
                totalDel,
                totalIns + totalSub + totalDel
                );

            // write the average line
            writer.WriteLine(" ,Average,{0},{1},{2},{3}",
                totalIns / totalCount,
                totalSub / totalCount,
                totalDel / totalCount,
                (totalIns + totalSub + totalDel) / totalCount
                );

            return true;
        }

        /// <summary>
        /// Determines the display string for the character-level error table. Most
        /// characters are just shown as-is, but a few of them are shown as strings
        /// for better readability.
        /// </summary>
        /// <param name="ch">The character to convert to a display string.</param>
        /// <returns>A display string for the character.</returns>
        protected virtual string CharToDisplay(char ch)
        {
            string s;
            switch (ch)
            {
                case ' ':
                    s = "Space";
                    break;
                case '–':
                    s = "Insertion";
                    break;
                case '¤': // OOCS
                    s = "Other";
                    break;
                default:
                    s = ch.ToString();
                    break;
            }
            return s;
        }

        #endregion

    }
}
