using System;
using System.IO;
using System.Text;
using System.Reflection;

namespace TextTest
{
	public class Phrases
	{
		private string[] _phrases;
		private int _lastIdx;
		private Random _rand;

        /// <summary>
        /// 
        /// </summary>
		public Phrases() 
            : this(String.Empty)
		{
            // this version loads the embedded resource "phrases.txt"
            // change to load from corpus
            _rand = new Random();
            _lastIdx = -1;
            this.Load("corpus.txt");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
		public Phrases(string file)
		{
			_rand = new Random();
			_lastIdx = -1;
			this.Load(file);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public bool Load()
		{
			return Load(String.Empty);
		}

        /// <summary>
        /// TODO: Consider updating the character set to reflect only the characters in the presented phrases.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
		public bool Load(string file)
		{
			bool success = true;
			StreamReader reader = null;
			try
			{
				string line;
				int i = 0;
				int count = 0;

				if (file == String.Empty)
				{
					Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TextTest.rsc.phrases.txt");
					reader = new StreamReader(stream, Encoding.ASCII);
				}
				else // load from a file path
				{
					reader = new StreamReader(file, Encoding.ASCII);
				}

				while ((line = reader.ReadLine()) != null)
				{
					line = line.Trim();
					if (line != String.Empty)
						count++; // one pass to just count the lines
				}
				reader.BaseStream.Position = 0; // reset position at the start
				_phrases = new string[count];
				while ((line = reader.ReadLine()) != null)
				{
					line = line.Trim();
					if (line != String.Empty)
						_phrases[i++] = line; // a second pass to store them up
				}
				reader.Close();
				reader = null;
			}
			catch (FileNotFoundException fnfex)
			{
				Console.WriteLine(fnfex.Message);
				success = false;
			}
            catch (IOException ioex)
            {
                Console.WriteLine(ioex.Message);
                success = false;
            }
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
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
        /// 
        /// </summary>
        /// <param name="lowercase"></param>
        /// <returns></returns>
		public string GetRandomPhrase(bool lowercase)
		{
			int index = _rand.Next(0, _phrases.Length);
			string phrase = GetPhraseAt(index);
			if (lowercase)
				return phrase.ToLower();
			return phrase;
		}

        /// <summary>
        /// 
        /// </summary>
		public string PreviousPhrase
		{
			get
			{
				return _phrases[_lastIdx];
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public string GetPhraseAt(int index)
		{
			if (0 <= index && index < _phrases.Length)
			{
				_lastIdx = index;
				return _phrases[index];
			}
			return String.Empty;
		}

        /// <summary>
        /// 
        /// </summary>
		public int NumPhrases
		{
			get
			{
				return _phrases.Length;
			}
		}

	}
}
