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
    using System.Windows.Input;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string typingResults;
        public string TypingResults
        {
            get { return (string)this.typingResults; }
            set
            {
                typingResults = value;
                OnPropertyChanged(nameof(TypingResults));
            }
        }

        public KeyboardModel KeyboardKeysModel { get; set; }

        public ICommand DeleteCommand => new DelegateCommands(this.DeleteClick);
        public void DeleteClick(object param)
        {
            if (!string.IsNullOrEmpty(TypingResults))
            {
                TypingResults = TypingResults.Remove(TypingResults.Length - 1);
            }
        }

        public ICommand ChangeLangCommand => new DelegateCommands(this.ChangeLangClick);

        public void ChangeLangClick(object param)
        {
            this.KeyboardKeysModel.IsEngOther = !this.KeyboardKeysModel.IsEngOther;
            this.KeyboardKeysModel.ChangeButtonsContent();
        }

        public ICommand ShiftCommand => new DelegateCommands(this.ShiftClick);

        public void ShiftClick(object param)
        {
            this.KeyboardKeysModel.IsShift = !this.KeyboardKeysModel.IsShift;
            this.KeyboardKeysModel.ChangeButtonsContent();
        }

        //public ICommand DeleteCommand => new DelegateCommands(this.DeleteClick);

        //public void DeleteClick(object param)
        //{
        //    if (!string.IsNullOrEmpty(this.Model.Text)) this.Model.Text = this.Model.Text.Remove(this.Model.Text.Length - 1);
        //}

        public ICommand ButtonClickCommand => new DelegateCommands(this.ButtonClick);

        public void ButtonClick(object param)
        {
            //this.KeyboardKeysModel.Text += param.ToString();
            TypingResults += param.ToString();
        }

        public ICommand SpaceCommand => new DelegateCommands(this.SpaceClick);

        public void SpaceClick(object param)
        {
            //this.KeyboardKeysModel.Text += " ";
            TypingResults += " ";
        }

        public string StrTest => "A string for the test (typing results placeholder).";

        public MainWindowViewModel()
        {
            KeyboardKeysModel = new KeyboardModel();
            TypingResults = "This is a test!";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
