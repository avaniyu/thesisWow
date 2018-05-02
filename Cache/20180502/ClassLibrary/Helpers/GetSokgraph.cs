namespace ClassLibrary.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Generate sokgraph for a word from lexicon
    /// all the units used are pixel, (0, 0) is in the upper left, assume there's no groove between letter keys
    /// </summary>
    public class GetSokgraph
    {
        //private string wordFromLexicon;
        public List<List<int>> WordSokgraph { get; set; }

        public List<int> IndexList { get; set; }
        public List<List<int>> TestSampleBaseLettersCoordinates { get; set; }

        public GetSokgraph(string word)
        {
            this.WordSokgraph = GetASokgraph(word);
        }

        private List<List<int>> GetASokgraph(string word)
        {
            var lettersCoordinates = this.GetLettersCoordinates();
            var sokgraph = new List<List<int>>();
            for (int i = 0; i < word.Length; i++)
            {
                int alphabetIndex = word[i] - 'A' < 26
                    ? word[i] - 'A'
                    : word[i] - 'a';
                sokgraph.Add(new List<int>(lettersCoordinates[alphabetIndex]));
            }
            return sokgraph;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>letter keys centre position in alphabet order</returns>
        private List<List<int>> GetLettersCoordinates()
        {
            var letters = new[] { "qwertyuiop", "asdfghjkl", "zxcvbnm" };
            // set sample keyboard size specification
            int sampleScreenWidth = 1920;
            int sampleScreenHeight = 1200;
            int sampleLetterKeyWidth = 1920 / 15;
            int sampleLetterKeyHeight = sampleLetterKeyWidth;
            List<int> sampleQCentreCoordiates = new List<int>(new int[] { 220, 200 });
            List<int> sampleACentreCoordiates = new List<int>(new int[] { 250, 328 });
            List<int> sampleZCentreCoordiates = new List<int>(new int[] { 280, 456 });
            List<List<int>> sampleBaseLettersCoordinates = new List<List<int>>();
            // cleaner way to do it?
            sampleBaseLettersCoordinates.Add(sampleQCentreCoordiates);
            sampleBaseLettersCoordinates.Add(sampleACentreCoordiates);
            sampleBaseLettersCoordinates.Add(sampleZCentreCoordiates);
            this.TestSampleBaseLettersCoordinates = sampleBaseLettersCoordinates;
            
            // set user keyboard size specification
            //var screenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            //var screenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            var screenHeight = 1200;
            var screenWidth = 1920;
            float ratioHeight = (float)screenHeight / sampleScreenHeight;
            float ratioWidth = (float)screenWidth / sampleScreenWidth;
            var keyboardLettersCoordinates = new List<List<int>>();
            for (int i = 0; i < letters.Length; i++)
            {
                for (int j = 0; j < letters[i].Length; j++)
                {
                    keyboardLettersCoordinates.Add(new List<int>(new int[] { (int)((sampleBaseLettersCoordinates[i][0] + sampleLetterKeyWidth * j) * ratioWidth),
                        (int)((sampleBaseLettersCoordinates[i][1] + sampleLetterKeyHeight * j) * ratioHeight)}));
                }
            }
            // keyboardIndex --> alphabetIndex
            string tmpLetters = "qwertyuiopasdfghjklzxcvbnm";
            string tmpAlphabet = "abcdefghijklmnopqrstuvwxyz";
            var alphabetCoordinates = new List<List<int>>();
            for (int i = 0; i < tmpAlphabet.Length; i++)
            {
                for (int j = 0; j < tmpLetters.Length; j++)
                {
                    if (tmpAlphabet[i] == tmpLetters[j])
                    {
                        alphabetCoordinates.Add(keyboardLettersCoordinates[j]);
                        break;
                    }
                }
            }
            return alphabetCoordinates;
        }

    }
}
