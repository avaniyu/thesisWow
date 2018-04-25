namespace TermControl.Models
{
    using System.ComponentModel;

    public class ButtonModel : INotifyPropertyChanged
    {
        private string content;
        public string Content
        {
            get { return this.content; }
            set {
                this.content = value;
                this.onPropertyChanged("Content");
            }
        }

        public string Name { get; set; }

        public int Column { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
