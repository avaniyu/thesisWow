using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermControls.Models
{
    public class KeyboardModelEngOther : KeyboardModel
    {
        public KeyboardModelEngOther() 
            : base()
        { }

        public override void InitContent()
        {
            //this.Content1 = new[] { "1234567890-", "qwertyuiop[]", "asdfghjkl;'", "zxcvbnm<>,.?" };
            //this.Content1Shift = new[] { "!@#$%^&*()-", "QWERTYUIOP{}", "ASDFGHJKL:\"", "ZXCVBNM<>,.?" };
            //// another language
            //this.Content2 = new[] { "1234567890-", "qwertyuiop[]", "asdfghjkl;'", "zxcvbnm<>,.?" };
            //this.Content2Shift = new[] { "!@#$%^&*()-", "QWERTYUIOP{}", "ASDFGHJKL:\"", "ZXCVBNM<>,.?" };

            this.Content1 = new[] { "qwertyuiop", "asdfghjkl", "zxcvbnm" };
            this.Content1Shift = new[] { "QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM" };
            // another language
            this.Content2 = new[] {"qwertyuiop", "asdfghjkl", "zxcvbnm" };
            this.Content2Shift = new[] {"QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM" };

        }
    }
}
