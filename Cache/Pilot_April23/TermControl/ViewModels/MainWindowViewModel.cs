namespace TermControl.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TermControl.Models;
    using TermControl.Commands;
    using System.ComponentModel;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
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

        public string StrTest => "A string for the test (typing results placeholder).";

        public MainWindowModel MainWindowModel { get; set; }

        public KeyboardKeysViewModel KeyboardKeysViewModel { get; set; }
        public DeleteViewModel DeleteViewModel { get; set; }

        public MainWindowViewModel()
        {
            MainWindowModel MainWindowModel = new MainWindowModel();
            this.typingResults = MainWindowModel.TypingResults;
            this.KeyboardKeysViewModel = new KeyboardKeysViewModel(this.typingResults);
            this.DeleteViewModel = new DeleteViewModel(this.typingResults);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
