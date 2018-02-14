using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextTest
{
    /// <summary>
    /// A class representing a set of characters. The default character set is the set of 
    /// alphanumeric characters and space.
    /// </summary>
    public class Charset : IEnumerable<char>
    {
        /// <summary>
        /// The underlying character set encapsulated by this class.
        /// </summary>
        private List<char> _charset;

        /// <summary>
        /// Creates a new default character set consisting of space and
        /// the alphanumeric characters.
        /// </summary>
        /// <remarks>
        /// A special ASCII character is reserved for out-of-character-set (OOCS)
        /// characters. For example, with the default character set, punctuation
        /// is all OOCS. Such characters can be grouped under the following special
        /// character:
        /// 
        ///     ¤ (164)     OOCS character
        /// 
        /// This character is not added to the default character set already, but
        /// clients can choose to add it. 
        /// </remarks>
        public Charset()
        {
            _charset = new List<char>();

            _charset.Add(' '); // space             // 1
            for (char ch = 'A'; ch <= 'Z'; ch++)    // 26
                _charset.Add(ch);
            for (char ch = 'a'; ch <= 'z'; ch++)    // 26
                _charset.Add(ch);
            for (char ch = '0'; ch <= '9'; ch++)    // 10  
                _charset.Add(ch);
        }

        /// <summary>
        /// Returns the current character set to its default state, which is space and
        /// the alphanumerics.
        /// </summary>
        public void RestoreDefaults()
        {
            _charset.Clear();

            _charset.Add(' '); // space             // 1
            for (char ch = 'A'; ch <= 'Z'; ch++)    // 26
                _charset.Add(ch);
            for (char ch = 'a'; ch <= 'z'; ch++)    // 26
                _charset.Add(ch);
            for (char ch = '0'; ch <= '9'; ch++)    // 10  
                _charset.Add(ch);
        }

        /// <summary>
        /// Clears the given character set.
        /// </summary>
        public void Clear()
        {
            _charset.Clear();
        }

        /// <summary>
        /// Indexer to get or set the character at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char this[int index]
        {
            get
            {
                return _charset[index];
            }
            set
            {
                _charset[index] = value;
            }
        }

        /// <summary>
        /// Returns the first index of a given character, or -1 if not found.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public int IndexOf(char ch)
        {
            int index = -1;
            for (int i = 0; i < _charset.Count; i++)
            {
                if (_charset[i] == ch)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// Returns true if the this character set contains the given character; false otherwise.
        /// </summary>
        /// <param name="ch">The given character.</param>
        /// <returns></returns>
        public bool Contains(char ch)
        {
            return (this.IndexOf(ch) != -1);
        }

        /// <summary>
        /// Sets the entire character set to the array of characters given.
        /// </summary>
        /// <param name="chars"></param>
        public void SetAll(char[] chars)
        {
            _charset.Clear();
            _charset.AddRange(chars);
        }

        /// <summary>
        /// Sets the entire character set to the list of characters given.
        /// </summary>
        /// <param name="chars"></param>
        public void SetAll(List<char> chars)
        {
            _charset.Clear();
            _charset.AddRange(chars);
        }

        /// <summary>
        /// Inserts the given character at the given index within the character set.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ch"></param>
        public void Insert(int index, char ch)
        {
            _charset.Insert(index, ch);
        }

        /// <summary>
        /// Appends the given character to the end of the character set.
        /// </summary>
        /// <param name="ch"></param>
        public void Add(char ch)
        {
            _charset.Add(ch);
        }

        /// <summary>
        /// Removes the first instance of the given character and returns the index
        /// at which it was removed, or -1 if the character was not found.
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public int Remove(char ch)
        {
            int index = -1;
            for (int i = 0; i < _charset.Count; i++)
            {
                if (_charset[i] == ch)
                {
                    _charset.RemoveAt(i);
                    index = i;
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// Removes the given character at the given index.
        /// </summary>
        /// <param name="index">The 0-based index of the character to remove.</param>
        /// <returns>The character removed.</returns>
        public char RemoveAt(int index)
        {
            char ch = _charset[index];
            _charset.RemoveAt(index);
            return ch;
        }

        /// <summary>
        /// Gets the number of entries in the character set.
        /// </summary>
        public int Count
        {
            get
            {
                return _charset.Count;
            }
        }

        #region IEnumerable

        /// <summary>
        /// Gets the type-specific enumerator for IEnumerable.
        /// </summary>
        /// <returns></returns>
        /// <see cref="https://msdn.microsoft.com/en-us/library/9eekhta0(v=vs.110).aspx"/>
        public IEnumerator<char> GetEnumerator()
        {
            return _charset.GetEnumerator();
        }

        /// <summary>
        /// Implements the generic interface for IEnumerable.
        /// </summary>
        /// <returns></returns>
        /// <see cref="https://msdn.microsoft.com/en-us/library/9eekhta0(v=vs.110).aspx"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _charset.GetEnumerator();
        }

        #endregion
    }
}
