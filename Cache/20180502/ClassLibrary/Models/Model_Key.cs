namespace ClassLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ClassLibrary.Helpers;

    public class Model_Key: Base_Binding
    {
        private string content;
        public string Content
        {
            get { return (string)this.content; }
            set
            {
                this.content = value;
                this.OnPropertyChanged(nameof(Content));
            }
        }

        public string Name { get; set; }
        public int Column { get; set; }


    }
}
