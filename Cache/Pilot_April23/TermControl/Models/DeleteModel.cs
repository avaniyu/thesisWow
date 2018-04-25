namespace TermControl.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TermControl.Models;

    public class DeleteModel : INotifyPropertyChanged
    {
        public DeleteModel()
        {

        }

        private string typingResults;

        public string TypingResults
        {
            get { return (string)this.typingResults; }
            set
            {
                this.typingResults = value;
                this.OnPropertyChanged("TypingResults");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
