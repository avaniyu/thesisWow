using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class Model_Keys_Localization : Model_Keys
    {
        public Model_Keys_Localization() : base() 
        {
        }

        public override void InitContent()
        {
            this.Content1 = new[] { "qwertyuiop", "asdfghjkl", "zxcvbnm" };
            this.Content1Shift = new[] { "QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM" };
            //another language
            this.Content2 = new[] { "qwertyuiop", "asdfghjkl", "zxcvbnm" };
            this.Content2Shift = new[] { "QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM" };
        }
    }
}
