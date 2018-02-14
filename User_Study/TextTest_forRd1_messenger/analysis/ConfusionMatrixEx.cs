using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TextTest
{
    public class ConfusionMatrixEx : ConfusionMatrix
    {
        #region Constructor

        /// <summary>
        /// Creates a confusion matrix showing the (intended, entered) characters along with
        /// insertion and omission errors.
        /// </summary>
        /// <remarks>The matrix is as defined by Wobbrock & Myers in their 2006 TOCHI 
        /// article entitled, "Analyzing the input stream for character-level errors in 
        /// unconstrained text entry evaluations." http://dl.acm.org/citation.cfm?id=1188819 
        /// </remarks>
        public ConfusionMatrixEx()
        {
            // do nothing
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Process the presented and transcribed strings, and the input stream, to determine the character-level 
        /// errors therein. Updates the counts in the confusion matrix representing these errors, weighted by the number 
        /// of optimal string alignments exist for P and T. Note that this is essentially the same algorithm as 
        /// CharacterErrorTableEx.RecordEntries, but instead of tallying errors for a table, we are talling them in a 
        /// confusion matrix. (Refactoring should put this algorithm in one place for both CharacterErrorTableEx and 
        /// ConfusionMatrixEx to use, but for now, the algorithm is just used in both places with minor changes.)
        /// </summary>
        /// <param name="P">The presented string, now stream-aligned with T and IS.</param>
        /// <param name="T">The transcribed string, now stream-aligned with P and IS.</param>
        /// <param name="IS">The input stream, now stream-aligned with P and T.</param>
        /// <param name="nAlign">The number of optimal alignments for P and T.</param>
        /// <remarks>This is the main algorithm described in the 2006 TOCHI journal article by
        /// Wobbrock & Myers. See http://dl.acm.org/citation.cfm?id=1188819 </remarks>
        public override void RecordEntries(string P, string T, string IS, int nAlign)
        {
            Debug.Assert(P.Length == T.Length && P.Length == IS.Length, "P, T, and IS are not stream-aligned in ConfusionMatrixEx.RecordEntries.");

            List<byte> bitflags = FlagStream(IS); // flag the input stream (mark the transcribed characters in IS)
            List<int> posvals = AssignPositionValues(IS, bitflags); // assign position values within substrings of IS
            
            for (int b = 0, a = 0; b < IS.Length; b++) // now determine errors
            {
                if (T[b] == '–') // (char) 150
                {
                    RecordEntry(_matrix, P[b], '–', nAlign); // (char) 150
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
                                RecordEntry(_matrix, P[target], IS[i], nAlign); // intended P[target], produced IS[i]
                            }
                            else if (target >= P.Length
                                    || (nextIS < IS.Length && IS[nextIS] == P[target])
                                    || (prevIS >= 0 && prevP >= 0 && IS[prevIS] == IS[i] && IS[prevIS] == P[prevP]))
                            {
                                RecordEntry(_matrix, '–', IS[i], nAlign); // (char) 150
                                I.Add(v);
                            }
                            else if (nextP < P.Length && IS[i] == P[nextP] && IsCharacter(T[target]))
                            {
                                RecordEntry(_matrix, P[target], '–', nAlign);
                                RecordEntry(_matrix, P[nextP], IS[i], nAlign);
                                M.Add(v);
                            }
                            else
                            {
                                RecordEntry(_matrix, P[target], IS[i], nAlign);
                            }
                        }
                    } // end a to b

                    if (P[b] == '–') // (char) 150
                        RecordEntry(_matrix, '–', T[b], nAlign); // (char) 150
                    else if (P[b] != T[b])
                        RecordEntry(_matrix, P[b], T[b], nAlign);
                    else if (P[b] != '·') // (char) 183
                        RecordEntry(_matrix, P[b], T[b], nAlign);

                    a = b + 1; // update the IS[a...b] substring range each loop
                }
            }
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
            Debug.Assert(IS.Length == bitflags.Count, "IS and bitflags are out of sync in ConfusionMatrixEx.AssignPositionValues.");

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

        #endregion

        #region File Writing

        /// <summary>
        /// Writes out a CSV file representing a confusion matrix with rows indicating the
        /// intended characters and columns representing the entered characters. 
        /// </summary>
        /// <param name="writer">A file writer to do the writing.</param>
        /// <returns>True; it is assumed that the encompassing writer will use try/catch/finally
        /// to catch any exceptions that may occur during the writing process.</returns>
        /// <remarks>The matrix is as defined by Wobbrock & Myers in their 2006 TOCHI 
        /// article entitled, "Analyzing the input stream for character- level errors in unconstrained 
        /// text entry evaluations." http://dl.acm.org/citation.cfm?id=1188819 </remarks>
        public override bool WriteToCsv(StreamWriter writer)
        {
            writer.WriteLine("Credit: http://dx.doi.org/10.1145/1188816.1188819");
            writer.WriteLine("ROWS = intended, COLUMNS = entered");

            // write the top row (columns) of characters
            foreach (char e in _charset)
            {
                if (e == ' ')
                    writer.Write(",Space");
                else if (e == '–') // (char) 150
                    writer.Write(",Omission");
                else if (e == '¤') // (char) 164
                    writer.Write(",Other");
                else
                    writer.Write(",{0}", e); // write the entered char
            }
            writer.WriteLine();

            // now write each row
            foreach (char i in _charset)
            {
                if (i == ' ')
                    writer.Write("Space");
                else if (i == '–') // (char) 150
                    writer.Write("Insertion");
                else if (i == '¤') // (char) 164
                    writer.Write("Other");
                else
                    writer.Write("{0}", i); // write the intended char

                // now write the acutal confusion counts
                foreach (char t in _charset)
                    writer.Write(",{0}", _matrix[i][t]);
                writer.WriteLine();
            }

            return true;
        }

        #endregion

    }
}
