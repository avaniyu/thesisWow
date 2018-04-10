namespace TermControls.Models
{
    using System.ComponentModel;

    public class ButtonModel : INotifyPropertyChanged
    {
        private string content;
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
                this.OnPropertyChanged("Content");
            }
        }
        public string Name { get; set; }
        public int Column { get; set; }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
