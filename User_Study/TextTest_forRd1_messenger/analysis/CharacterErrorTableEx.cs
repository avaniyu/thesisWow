using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TextTest
{
    public class CharacterErrorTableEx : CharacterErrorTable
    {
        #region Fields

        // inherited members: presented chars; and uncorrected insertions, substitutions, and omissions
        protected Dictionary<char, double> _transcribedChars;
        protected Dictionary<char, double> _enteredChars;
        protected Dictionary<char, double> _uncNoErrors;
        protected Dictionary<char, double> _corNoErrors;
        protected Dictionary<char, double> _corInsertions;
        protected Dictionary<char, double> _corSubstitutions;
        protected Dictionary<char, double> _corOmissions;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a character-level error table that shows errors and ratios among
        /// character-level errors for substitution, omission, and insertion errors.
        /// </summary>
        /// <remarks>The table is as defined by Wobbrock & Myers in their 2006 TOCHI 
        /// article entitled, "Analyzing the input stream for character- level errors in 
        /// unconstrained text entry evaluations." http://dl.acm.org/citation.cfm?id=1188819 </remarks>
        public CharacterErrorTableEx()
        {
            // remove the insertion/omission character from our character set,
            // as it does not play a role in the Wobbrock & Myers table.
            _charset.Remove('–');           // (char) 150
            _presentedChars.Remove('–');    // (char) 150
            _uncInsertions.Remove('–');     // (char) 150
            _uncSubstitutions.Remove('–');  // (char) 150
            _uncOmissions.Remove('–');      // (char) 150

            // create new Dictionaries for other entries
            _transcribedChars = new Dictionary<char, double>(_charset.Count);
            _enteredChars = new Dictionary<char, double>(_charset.Count);
            _uncNoErrors = new Dictionary<char, double>(_charset.Count);
            _corNoErrors = new Dictionary<char, double>(_charset.Count);
            _corInsertions = new Dictionary<char, double>(_charset.Count);
            _corSubstitutions = new Dictionary<char, double>(_charset.Count);
            _corOmissions = new Dictionary<char, double>(_charset.Count);

            // initialize the new Dictionaries with the charset
            foreach (char ch in _charset)
            {
                _transcribedChars.Add(ch, 0.0);
                _enteredChars.Add(ch, 0.0);
                _uncNoErrors.Add(ch, 0.0);
                _corNoErrors.Add(ch, 0.0);
                _corInsertions.Add(ch, 0.0);
                _corSubstitutions.Add(ch, 0.0);
                _corOmissions.Add(ch, 0.0);
            }
        }

        /// <summary>
        /// Clears all character counts and resets them for each character to zero.
        /// </summary>
        protected override void Clear()
        {
            base.Clear();

            _transcribedChars.Clear();
            _enteredChars.Clear();
            _uncNoErrors.Clear();
            _corNoErrors.Clear();
            _corInsertions.Clear();
            _corSubstitutions.Clear();
            _corOmissions.Clear();

            foreach (char ch in _charset)
            {
                _transcribedChars.Add(ch, 0.0);
                _enteredChars.Add(ch, 0.0);
                _uncNoErrors.Add(ch, 0.0);
                _corNoErrors.Add(ch, 0.0);
                _corInsertions.Add(ch, 0.0);
                _corSubstitutions.Add(ch, 0.0);
                _corOmissions.Add(ch, 0.0);
            }
        }

        #endregion

        #region Record Entries

        /// <summary>
        /// Process the presented and transcribed strings, and the input stream, to determine the character-level 
        /// errors therein. Updates the counts for each character's error types, weighted by the number of
        /// optimal string alignments exist for P and T. 
        /// </summary>
        /// <param name="P">The presented string, now stream-aligned with T and IS.</param>
        /// <param name="T">The transcribed string, now stream-aligned with P and IS.</param>
        /// <param name="IS">The input stream, now stream-aligned with P and T.</param>
        /// <param name="nAlign">The number of optimal alignments for P and T.</param>
        /// <remarks>This is the main algorithm described in the 2006 TOCHI journal article by
        /// Wobbrock & Myers. See http://dl.acm.org/citation.cfm?id=1188819 </remarks>
        public override void RecordEntries(string P, string T, string IS, int nAlign)
        {
            Debug.Assert(P.Length == T.Length && P.Length == IS.Length, "P, T, and IS are not stream-aligned in CharacterErrorTableEx.RecordEntries.");

            CountPresentedTranscribedEnteredCharacters(P, T, IS, nAlign); // counts all chars in P, T, and IS

            List<byte> bitflags = FlagStream(IS); // flag the input stream (mark the transcribed characters in IS)
            List<int> posvals = AssignPositionValues(IS, bitflags); // assign position values within substrings of IS
            //DebugWriteStringsFlagsPositionValues(P, T, IS, bitflags, posvals); // for debugging

            for (int b = 0, a = 0; b < IS.Length; b++) // now determine errors
            {
                if (T[b] == '–') // (char) 150
                {
                    RecordEntry(_uncOmissions, P[b], nAlign); // omitted P[b]
                }
                else if (bitflags[b] == 0x01 || b == IS.Length - 1)
                {
                    HashSet<int> M = new HashSet<int>();    // corrected oMissions
                    HashSet<int> I = new HashSet<int>();    // corrected Insertions

                    for (int i = a; i < b; i++)             // loop over a substring between flags
                    {
                        int v = posvals[i];
                        if (IS[i] == '‹') // (char) 139
                        {
                            if (M.Contains(v))
                                M.Remove(v);
                            if (I.Contains(v))
                                I.Remove(v);
                        }
                        else if (IS[i] != '·') // (char) 183
                        {
                            int target = LookAhead(P, b, v + M.Count - I.Count, new CharTestFn(IsCharacter));
                            int nextP = LookAhead(P, target, 1, new CharTestFn(IsCharacter));
                            int prevP = LookBehind(P, target, 1, new CharTestFn(IsCharacter));
                            int nextIS = LookAhead(IS, i, 1, new CharTestFn(IsNotSpacer));
                            int prevIS = LookBehind(IS, i, 1, new CharTestFn(IsNotSpacer));

                            if (target < P.Length && IS[i] == P[target])
                            {
                                RecordEntry(_corNoErrors, IS[i], nAlign);
                            }
                            else if (target >= P.Length
                                    || (nextIS < IS.Length && IS[nextIS] == P[target])
                                    || (prevIS >= 0 && prevP >= 0 && IS[prevIS] == IS[i] && IS[prevIS] == P[prevP]))
                            {
                                RecordEntry(_corInsertions, IS[i], nAlign);           // for insertions, add produced char IS[i]
                                I.Add(v);
                            }
                            else if (nextP < P.Length && IS[i] == P[nextP] && IsCharacter(T[target]))
                            {
                                RecordEntry(_corOmissions, P[target], nAlign);
                                RecordEntry(_corNoErrors, IS[i], nAlign);             // intended P[nextP], produced IS[i]
                                M.Add(v);
                            }
                            else
                            {
                                RecordEntry(_corSubstitutions, P[target], nAlign);    // intended P[target], produced IS[i]
                            }
                        }
                    } // end a to b

                    if (P[b] == '–') // (char) 150
                        RecordEntry(_uncInsertions, T[b], nAlign);    // for insertions, add produced char T[b]      
                    else if (P[b] != T[b])
                        RecordEntry(_uncSubstitutions, P[b], nAlign); // intended P[b], produced T[b]
                    else if (P[b] != '·') // (char) 183
                        RecordEntry(_uncNoErrors, P[b], nAlign);      // intended P[b], produced T[b]

                    a = b + 1; // update the IS[a...b] substring range each loop
                }
            }
        }

        /// <summary>
        /// Counts the characters in the presented and transcribed strings, and in the input stream.
        /// The characters are weighted by the number of optimal alignments for a given P, T, and IS.
        /// </summary>
        /// <param name="P">The presented string.</param>
        /// <param name="T">The transcribed string.</param>
        /// <param name="IS">The input stream.</param>
        /// <param name="nAlign">The number of optimal alignments being processed for a given P, T, 
        /// and IS.</param>
        private void CountPresentedTranscribedEnteredCharacters(string P, string T, string IS, int nAlign)
        {
            // do weighted counting of the presented characters
            for (int i = 0; i < P.Length; i++)
            {
                if (P[i] != '–' && P[i] != '·') // (char) 150, (char) 183
                    RecordEntry(_presentedChars, P[i], nAlign);
            }

            // do weighted counting of the transcribed characters
            for (int i = 0; i < T.Length; i++)
            {
                if (T[i] != '–' && T[i] != '·') // (char) 150, (char) 183
                    RecordEntry(_transcribedChars, T[i], nAlign);
            }

            // do weighted counting of the input stream characters
            for (int i = 0; i < IS.Length; i++)
            {
                if (IS[i] != '‹' && IS[i] != '·') // (char) 139, (char) 183
                    RecordEntry(_enteredChars, IS[i], nAlign);
            }
        }

        /// <summary>
        /// Writes to the console the stream-aligned presented, transcribed, and input stream strings, 
        /// and the bitflags indicating which characters in the input stream are transcribed, and the
        /// position values indicating, for each corrected substring in IS, the position index of each 
        /// character. For debugging.
        /// </summary>
        /// <param name="P"></param>
        /// <param name="T"></param>
        /// <param name="IS"></param>
        /// <param name="bitflags"></param>
        /// <param name="posvals"></param>
        private void DebugWriteStringsFlagsPositionValues(string P, string T, string IS, List<byte> bitflags, List<int> posvals)
        {
            Debug.WriteLine(" P: {0}\r\n T: {1}\r\nIS: {2}", P, T, IS);

            Debug.Write("    ");
            for (int i = 0; i < bitflags.Count; i++)
                Debug.Write(bitflags[i].ToString());
            Debug.WriteLine("");

            Debug.Write("    ");
            for (int i = 0; i < posvals.Count; i++)
                Debug.Write(posvals[i].ToString());
            Debug.WriteLine("\r\n");
        }

        /// <summary>
        /// Creates a list of bit flags that indicate which characters in IS belong to T; that
        /// is, are part of the final transcribed string.
        /// </summary>
        /// <param name="IS"></param>
        /// <returns></returns>
        private List<byte> FlagStream(string IS)
        {
            List<byte> bitflags = new List<byte>();
            if (IS == null || IS.Length == 0)
                return bitflags;

            bitflags.Capacity = IS.Length;
            for (int i = 0; i < IS.Length; i++)
                bitflags.Add(0x00); // set all to zero initially

            for (int i = IS.Length - 1, bksp = 0; i >= 0; i--)
            {
                if (IS[i] != '·') // (char) 183
                {
                    if (IS[i] == '‹') // (char) 139
                        bksp++;
                    else if (bksp == 0)
                        bitflags[i] = 0x01;
                    else
                        bksp--;
                }
            }
            return bitflags;
        }

        /// <summary>
        /// Position values help determine the intended target letter in P for each 
        /// letter in IS. Flagged letters always receive a position value of zero. 
        /// Within each unflagged substring between two flags in IS, a letter’s position 
        /// value is the substring index it would have had if the substring were 
        /// transcribed unto this point.A backspace’s position value, by contrast, is 
        /// the position value of the letter that the backspace erases.
        /// </summary>
        /// <param name="IS">The input stream.</param>
        /// <param name="bitflags">A list of bit flags indicating which characters in IS
        /// appear in the transcribed string, T.</param>
        /// <returns>A list of position values for every character position in IS.</returns>
        private List<int> AssignPositionValues(string IS, List<byte> bitflags)
        {
            Debug.Assert(IS.Length == bitflags.Count, "IS and bitflags are out of sync in CharacterErrorTableEx.AssignPositionValues.");

            List<int> posvals = new List<int>(IS.Length);
            for (int i = 0; i < IS.Length; i++)
                posvals.Add(0);
            if (IS == null || IS.Length == 0 || bitflags == null || bitflags.Count == 0)
                return posvals;

            for (int i = 0, pos = 0; i < IS.Length; i++)
            {
                if (bitflags[i] == 0x01)
                {
                    posvals[i] = 0;
                    pos = 0;
                }
                else // IS[i] not in transcribed string
                {
                    if (IS[i] == '‹' && pos > 0) // (char) 139
                        pos--;
                    posvals[i] = pos;
                    if (IS[i] != '‹' && IS[i] != '·') // (char) 139, (char) 183
                        pos++;
                }
            }
            return posvals;
        }

        /// <summary>
        /// This method looks forward in string S from a zero-based index 'start' until a 'count' 
        /// number of conditionFn's have been satisfied, returning the index of the count^th 
        /// successful test.
        /// </summary>
        /// <param name="S"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="conditionFn"></param>
        /// <returns></returns>
        private int LookAhead(string S, int start, int count, CharTestFn conditionFn)
        {
            int index = start;
            while (0 <= index && index < S.Length && !conditionFn(S[index]))
                index++;
            while (count > 0 && index < S.Length)
            {
                if (++index >= S.Length)
                    break;
                else if (conditionFn(S[index]))
                    count--;
            }
            return index;
        }

        /// <summary>
        /// Analogous to LookAhead but works in reverse.
        /// </summary>
        /// <param name="S"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="conditionFn"></param>
        /// <returns></returns>
        private int LookBehind(string S, int start, int count, CharTestFn conditionFn)
        {
            int index = start;
            while (0 <= index && index < S.Length && !conditionFn(S[index]))
                index--;
            while (count > 0 && index >= 0)
            {
                if (--index < 0)
                    break;
                else if (conditionFn(S[index]))
                    count--;
            }
            return index;
        }

        /// <summary>
        /// Defines a function type that returns a bool after testing a character 'ch' for
        /// certain properties.
        /// </summary>
        /// <param name="ch">The character to test.</param>
        /// <returns>True if 'ch' passes the test; false otherwise.</returns>
        private delegate bool CharTestFn(char ch);

        /// <summary>
        /// A character test function for determining whether the given character
        /// is not a backspace, insertion/omission spacer, or input stream spacer.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private bool IsCharacter(char ch)
        {
            return (ch != '‹' && ch != '–' && ch != '·'); // (char) 139, (char) 150, (char) 183
        }

        /// <summary>
        /// A character test function for determining whether the given character
        /// is a backspace.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private bool IsBackspace(char ch)
        {
            return (ch == '‹'); // (char) 139
        }

        /// <summary>
        /// A character test function for determining whether the given character
        /// is the insertion/omission spacer.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private bool IsInsertionOrOmission(char ch)
        {
            return (ch == '–'); // (char) 150
        }

        /// <summary>
        /// A character test function for determining whether the given character
        /// is the input stream spacer.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private bool IsNotSpacer(char ch)
        {
            return (ch != '·'); // (char) 183
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CalculateErrorProbabilities()
        {
            // do nothing
        }

        #endregion

        #region Character-Level Metrics

        /// <summary>
        /// Indicates whether a given character has any table entries in this table. To have
        /// table entries, it must have been presented and/or entered.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private bool HasTableEntries(char ch)
        {
            return (_presentedChars[ch] + _enteredChars[ch]) > 0.0;
        }

        /// <summary>
        /// Gets the number of times a given character was presented.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetNumPresented(char ch)
        {
            return _presentedChars[ch];
        }

        /// <summary>
        /// Gets the number of times that all characters were presented.
        /// </summary>
        /// <returns></returns>
        private double GetNumPresented()
        {
            double nPresented = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                    nPresented += this.GetNumPresented(ch);
            }
            return nPresented;
        }

        /// <summary>
        /// Gets the number of times a given character was transcribed.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetNumTranscribed(char ch)
        {
            return _transcribedChars[ch];
        }

        /// <summary>
        /// Gets the number of times that all characters were transcribed.
        /// </summary>
        /// <returns></returns>
        private double GetNumTranscribed()
        {
            double nTranscribed = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                    nTranscribed += this.GetNumTranscribed(ch);
            }
            return nTranscribed;
        }

        /// <summary>
        /// Gets the number of times a given character was entered.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetNumEntered(char ch)
        {
            return _enteredChars[ch];
        }

        /// <summary>
        /// Gets the number of times that all characters were entered.
        /// </summary>
        /// <returns></returns>
        private double GetNumEntered()
        {
            double nEntered = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                    nEntered += this.GetNumEntered(ch);
            }
            return nEntered;
        }

        /// <summary>
        /// Gets the number of times a given character was intended; that is, 
        /// the number of times it was presumably attempted to be entered. 
        /// Could also be named GetNumAttempted.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetNumIntended(char ch)
        {
            return _uncNoErrors[ch]
                + _corNoErrors[ch]
                + _uncSubstitutions[ch]
                + _corSubstitutions[ch];
        }

        /// <summary>
        /// Gets the number of times that all characters were intended.
        /// </summary>
        /// <returns></returns>
        private double GetNumIntended()
        {
            double nIntended = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                    nIntended += this.GetNumIntended(ch);
            }
            return nIntended;
        }

        /// <summary>
        /// Gets the number of times a given character was entered correctly.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetNumCorrect(char ch)
        {
            return _uncNoErrors[ch] + _corNoErrors[ch];
        }

        /// <summary>
        /// Gets the number of times that all characters were entered correctly.
        /// </summary>
        /// <returns></returns>
        private double GetNumCorrect()
        {
            double nCorrect = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                    nCorrect += this.GetNumCorrect(ch);
            }
            return nCorrect;
        }

        /// <summary>
        /// Gets the uncorrected error rate for a given character. 
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetUncorrectedErrorRate(char ch)
        {
           return (_transcribedChars[ch] > 0.0) ? 1.0 - (_uncNoErrors[ch] / _transcribedChars[ch]) : 0.0;
        }

        /// <summary>
        /// Gets the overall uncorrected error rate.
        /// </summary>
        /// <returns></returns>
        private double GetUncorrectedErrorRate()
        {
            double nUncNoErrors = 0.0;
            double nTranscribed = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nUncNoErrors += _uncNoErrors[ch];
                    nTranscribed += _transcribedChars[ch];
                }
            }
            return (nTranscribed > 0.0) ? 1.0 - (nUncNoErrors / nTranscribed) : 0.0;
        }

        /// <summary>
        /// Gets the corrected error rate for a given character.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetCorrectedErrorRate(char ch)
        {
            double correctedChars = (_enteredChars[ch] - _transcribedChars[ch]);
            return (correctedChars > 0.0) ? 1.0 - (_corNoErrors[ch] / correctedChars) : 0.0;
        }

        /// <summary>
        /// Gets the overall corrected error rate.
        /// </summary>
        /// <returns></returns>
        private double GetCorrectedErrorRate()
        {
            double nCorNoErrors = 0.0;
            double correctedChars = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nCorNoErrors += _corNoErrors[ch];
                    correctedChars += (_enteredChars[ch] - _transcribedChars[ch]);
                }
            }
            return (correctedChars > 0.0) ? 1.0 - (nCorNoErrors / correctedChars) : 0.0;
        }

        /// <summary>
        /// Gets the total error rate for a given character.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetTotalErrorRate(char ch)
        {
            return (_enteredChars[ch] > 0.0) ? 1.0 - ((_uncNoErrors[ch] + _corNoErrors[ch]) / _enteredChars[ch]) : 0.0;
        }

        /// <summary>
        /// Gets the overall total error rate.
        /// </summary>
        /// <returns></returns>
        private double GetTotalErrorRate()
        {
            double nUncNoErrors = 0.0;
            double nCorNoErrors = 0.0;
            double nEntered = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nUncNoErrors += _uncNoErrors[ch];
                    nCorNoErrors += _corNoErrors[ch];
                    nEntered += _enteredChars[ch];
                }
            }
            return (nEntered > 0.0) ? 1.0 - ((nUncNoErrors + nCorNoErrors) / nEntered) : 0.0;
        }

        /// <summary>
        /// Gets the number of substitutions for a given character.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetNumSubstitutions(char ch)
        {
            return _uncSubstitutions[ch] + _corSubstitutions[ch];
        }

        /// <summary>
        /// Gets the number of substitutions for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetNumSubstitutions()
        {
            double nSubstitutions = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                    nSubstitutions += this.GetNumSubstitutions(ch);
            }
            return nSubstitutions;
        }

        /// <summary>
        /// Gets the uncorrected substitution rate for a given character. This is the ratio of uncorrected
        /// substitutions to intended characters. It answers the question, "when trying for i, what is the 
        /// probability that we produce an uncorrected substitution for i?"
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetUncorrectedSubstitutionRate(char ch)
        {
            double nIntended = this.GetNumIntended(ch);
            return (nIntended > 0.0) ? _uncSubstitutions[ch] / nIntended : 0.0;
        }

        /// <summary>
        /// Gets the uncorrected substitution rate for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetUncorrectedSubstitutionRate()
        {
            double nUncSubstitutions = 0.0;
            double nIntended = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nUncSubstitutions += _uncSubstitutions[ch];
                    nIntended += this.GetNumIntended(ch);
                }
            }
            return (nIntended > 0.0) ? nUncSubstitutions / nIntended : 0.0;
        }

        /// <summary>
        /// Gets the corrected substitution rate for a given character. This is the ratio of corrected
        /// substitutions to intended characters. It answers the question, "when trying for i, what is the 
        /// probability that we produce a corrected substitution for i?"
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetCorrectedSubstitutionRate(char ch)
        {
            double nIntended = this.GetNumIntended(ch);
            return (nIntended > 0.0) ? _corSubstitutions[ch] / nIntended : 0.0;
        }

        /// <summary>
        /// Gets the corrected substitution rate for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetCorrectedSubstitutionRate()
        {
            double nCorSubstitutions = 0.0;
            double nIntended = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nCorSubstitutions += _corSubstitutions[ch];
                    nIntended += this.GetNumIntended(ch);
                }
            }
            return (nIntended > 0.0) ? nCorSubstitutions / nIntended : 0.0;
        }

        /// <summary>
        /// Gets the total substitution rate for a given character. This is the ratio all
        /// substitutions to intended characters. It answers the question, "when trying for i,
        /// what is the probability that we don't get i?"
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetTotalSubstitutionRate(char ch)
        {
            double nIntended = this.GetNumIntended(ch);
            return (nIntended > 0.0) ? (_uncSubstitutions[ch] + _corSubstitutions[ch]) / nIntended : 0.0;
        }

        /// <summary>
        /// Gets the total substitution rate for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetTotalSubstitutionRate()
        {
            double nUncSubstitutions = 0.0;
            double nCorSubstitutions = 0.0;
            double nIntended = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nUncSubstitutions += _uncSubstitutions[ch];
                    nCorSubstitutions += _corSubstitutions[ch];
                    nIntended += this.GetNumIntended(ch);
                }
            }
            return (nIntended > 0.0) ? (nUncSubstitutions + nCorSubstitutions) / nIntended : 0.0;
        }

        /// <summary>
        /// Gets the number of omissions for a given character.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetNumOmissions(char ch)
        {
            return _uncOmissions[ch] + _corOmissions[ch];
        }

        /// <summary>
        /// Gets the number of omissions for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetNumOmissions()
        {
            double nOmissions = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                    nOmissions += this.GetNumOmissions(ch);
            }
            return nOmissions;
        }

        /// <summary>
        /// Gets the uncorrected omission rate for a given character. This is the ratio of uncorrected
        /// omissions to presented characters. It answers the question, "when i is presented, what is the 
        /// probability that i is omitted and that omission remains uncorrected?"
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetUncorrectedOmissionRate(char ch)
        {
            double nPresented = this.GetNumPresented(ch);
            return (nPresented > 0.0) ? _uncOmissions[ch] / nPresented : 0.0;
        }

        /// <summary>
        /// Gets the uncorrected omission rate for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetUncorrectedOmissionRate()
        {
            double nUncOmissions = 0.0;
            double nPresented = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nUncOmissions += _uncOmissions[ch];
                    nPresented += this.GetNumPresented(ch);
                }
            }
            return (nPresented > 0.0) ? nUncOmissions / nPresented : 0.0;
        }

        /// <summary>
        /// Gets the corrected omission rate for a given character. This is the ratio of corrected
        /// omissions to presented characters. It answers the question, "when i is presented, what is the 
        /// probability that i is omitted and that omission is corrected?"
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetCorrectedOmissionRate(char ch)
        {
            double nPresented = this.GetNumPresented(ch);
            return (nPresented > 0.0) ? _corOmissions[ch] / nPresented : 0.0;
        }

        /// <summary>
        /// Gets the corrected omission rate for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetCorrectedOmissionRate()
        {
            double nCorOmissions = 0.0;
            double nPresented = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nCorOmissions += _corOmissions[ch];
                    nPresented += this.GetNumPresented(ch);
                }
            }
            return (nPresented > 0.0) ? nCorOmissions / nPresented : 0.0;
        }

        /// <summary>
        /// Gets the total omission rate for a given character. This is the ratio of all
        /// omissions to presented characters. It answers the question, "when i is presented, what is the 
        /// probability that i is omitted?"
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetTotalOmissionRate(char ch)
        {
            double nPresented = this.GetNumPresented(ch);
            return (nPresented > 0.0) ? (_uncOmissions[ch] + _corOmissions[ch]) / nPresented : 0.0;
        }

        /// <summary>
        /// Gets the total omission rate for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetTotalOmissionRate()
        {
            double nUncOmissions = 0.0;
            double nCorOmissions = 0.0;
            double nPresented = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nUncOmissions += _uncOmissions[ch];
                    nCorOmissions += _corOmissions[ch];
                    nPresented += this.GetNumPresented(ch);
                }
            }
            return (nPresented > 0.0) ? (nUncOmissions + nCorOmissions) / nPresented : 0.0;
        }

        /// <summary>
        /// Gets the number of insertions for a given character.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetNumInsertions(char ch)
        {
            return _uncInsertions[ch] + _corInsertions[ch];
        }

        /// <summary>
        /// Gets the number of insertions for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetNumInsertions()
        {
            double nInsertions = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                    nInsertions += this.GetNumInsertions(ch);
            }
            return nInsertions;
        }

        /// <summary>
        /// Gets the uncorrected insertion rate for a given character. This is the ratio of uncorrected
        /// insertions to entered characters. It answers the question, "when i is entered, what is the 
        /// probability that i was inserted and remains uncorrected?"
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetUncorrectedInsertionRate(char ch)
        {
            double nEntered = this.GetNumEntered(ch);
            return (nEntered > 0.0) ? _uncInsertions[ch] / nEntered : 0.0;
        }

        /// <summary>
        /// Gets the uncorrected insertion rate for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetUncorrectedInsertionRate()
        {
            double nUncInsertions = 0.0;
            double nEntered = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nUncInsertions += _uncInsertions[ch];
                    nEntered += this.GetNumEntered(ch);
                }
            }
            return (nEntered > 0.0) ? nUncInsertions / nEntered : 0.0;
        }

        /// <summary>
        /// Gets the corrected insertion rate for a given character. This is the ratio of corrected
        /// insertions to entered characters. It answers the question, "when i is entered, what is the 
        /// probability that i was inserted and was corrected?"
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetCorrectedInsertionRate(char ch)
        {
            double nEntered = this.GetNumEntered(ch);
            return (nEntered > 0.0) ? _corInsertions[ch] / nEntered : 0.0;
        }

        /// <summary>
        /// Gets the corrected insertion rate for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetCorrectedInsertionRate()
        {
            double nCorInsertions = 0.0;
            double nEntered = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nCorInsertions += _corInsertions[ch];
                    nEntered += this.GetNumEntered(ch);
                }
            }
            return (nEntered > 0.0) ? nCorInsertions / nEntered : 0.0;
        }

        /// <summary>
        /// Gets the total insertion rate for a given character. This is the ratio of all
        /// insertions to entered characters. It answers the question, "when i is entered, what is the 
        /// probability that i was inserted?"
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private double GetTotalInsertionRate(char ch)
        {
            double nEntered = this.GetNumEntered(ch);
            return (nEntered > 0.0) ? (_uncInsertions[ch] + _corInsertions[ch]) / nEntered : 0.0;
        }

        /// <summary>
        /// Gets the total insertion rate for all characters.
        /// </summary>
        /// <returns></returns>
        private double GetTotalInsertionRate()
        {
            double nUncInsertions = 0.0;
            double nCorInsertions = 0.0;
            double nEntered = 0.0;
            foreach (char ch in _charset)
            {
                if (Char.IsLetterOrDigit(ch) || ch == ' ' || ch == '¤') // (char) 164
                {
                    nUncInsertions += _uncInsertions[ch];
                    nCorInsertions += _corInsertions[ch];
                    nEntered += this.GetNumEntered(ch);
                }
            }
            return (nEntered > 0.0) ? (nUncInsertions + nCorInsertions) / nEntered : 0.0;
        }

        #endregion

        #region File Writing

        /// <summary>
        /// Writes the character-level error table as defined by Wobbrock & Myers in
        /// their 2006 TOCHI article entitled, "Analyzing the input stream for character-level 
        /// errors in unconstrained text entry evaluations." http://dl.acm.org/citation.cfm?id=1188816.1188819
        /// </summary>
        /// <param name="writer">A file writer to do the writing.</param>
        /// <param name="wantWholeCharset">If true, a table entry for every character in the
        /// possible presented-string character set is shown; if false, then only rows whose
        /// characters actually appeared in any of the presented strings are shown.</param>
        /// <returns>True; it is assumed that the encompassing writer will use try/catch/finally
        /// to catch any exceptions that may occur during the writing process.</returns>
        public override bool WriteToCsv(StreamWriter writer, bool wantWholeCharset)
        {
            writer.WriteLine("Credit: http://dx.doi.org/10.1145/1188816.1188819");
            writer.WriteLine(",COUNTS,,,,,ERROR RATES,,,SUBSTITUTIONS vs. INTENTIONS,,,,OMISSIONS vs. PRESENTATIONS,,,,INSERTIONS vs. ENTRIES,,,,");
            writer.WriteLine("Character,Presented,Transcribed,Entered,Intended,Correct,Unc,Cor,Total,Count,Unc,Cor,Total,Count,Unc,Cor,Total,Count,Unc,Cor,Total");
            foreach (char ch in _charset)
            {
                if (wantWholeCharset || HasTableEntries(ch))
                {
                     writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}",
                        this.CharToDisplay(ch),                     // Character
                        this.GetNumPresented(ch),                   // Presented
                        this.GetNumTranscribed(ch),                 // Transcribed
                        this.GetNumEntered(ch),                     // Entered
                        this.GetNumIntended(ch),                    // Intended
                        this.GetNumCorrect(ch),                     // Correct

                        this.GetUncorrectedErrorRate(ch),           // Unc
                        this.GetCorrectedErrorRate(ch),             // Cor
                        this.GetTotalErrorRate(ch),                 // Total

                        this.GetNumSubstitutions(ch),               // Count
                        this.GetUncorrectedSubstitutionRate(ch),    // Unc
                        this.GetCorrectedSubstitutionRate(ch),      // Cor
                        this.GetTotalSubstitutionRate(ch),          // Total

                        this.GetNumOmissions(ch),                   // Count
                        this.GetUncorrectedOmissionRate(ch),        // Unc
                        this.GetCorrectedOmissionRate(ch),          // Cor
                        this.GetTotalOmissionRate(ch),              // Total

                        this.GetNumInsertions(ch),                  // Count
                        this.GetUncorrectedInsertionRate(ch),       // Unc
                        this.GetCorrectedInsertionRate(ch),         // Cor
                        this.GetTotalInsertionRate(ch)              // Total
                        );
                }
            }

            // total calculations over the entire table are simply not
            // parameterized with a specific 'ch' argument to the fns.
            writer.WriteLine("Total,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}",
                this.GetNumPresented(),                   // Presented
                this.GetNumTranscribed(),                 // Transcribed
                this.GetNumEntered(),                     // Entered
                this.GetNumIntended(),                    // Intended
                this.GetNumCorrect(),                     // Correct

                this.GetUncorrectedErrorRate(),           // Unc
                this.GetCorrectedErrorRate(),             // Cor
                this.GetTotalErrorRate(),                 // Total

                this.GetNumSubstitutions(),               // Count
                this.GetUncorrectedSubstitutionRate(),    // Unc
                this.GetCorrectedSubstitutionRate(),      // Cor
                this.GetTotalSubstitutionRate(),          // Total

                this.GetNumOmissions(),                   // Count
                this.GetUncorrectedOmissionRate(),        // Unc
                this.GetCorrectedOmissionRate(),          // Cor
                this.GetTotalOmissionRate(),              // Total

                this.GetNumInsertions(),                  // Count
                this.GetUncorrectedInsertionRate(),       // Unc
                this.GetCorrectedInsertionRate(),         // Cor
                this.GetTotalInsertionRate()              // Total
                );

            return true;
        }

        #endregion
    }
}