namespace ClassLibrary.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ClassLibrary.Models;
    using ClassLibrary.Commands;
    using System.ComponentModel;
    using System.Windows.Input;
    using ClassLibrary.Helpers;

    public class ViewModel_Main : Base_Binding
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

        public Model_Keys_Localization Model { get; set; }

        #region for Deletion

        public ICommand DeleteCommand => new DelegateCommands(this.DeleteClick);
        public void DeleteClick(object param)
        {
            if (!string.IsNullOrEmpty(this.TypingResults))
            {
                this.TypingResults = this.TypingResults.Remove(this.TypingResults.Length - 1);
            }
        }

        #endregion

        #region for keyboard keys

        public ICommand ChangeLangCommand => new DelegateCommands(this.ChangeLangClick);

        public void ChangeLangClick(object param)
        {
            this.Model.IsEngOther = !this.Model.IsEngOther;
            this.Model.ChangeKeysContent();
        }

        public ICommand ShiftCommand => new DelegateCommands(this.ShiftClick);

        public void ShiftClick(object param)
        {
            this.Model.IsShift = !this.Model.IsShift;
            this.Model.ChangeKeysContent();
        }

        public ICommand KeyClickCommand => new DelegateCommands(this.KeyClick);

        public void KeyClick(object param)
        {
            this.TypingResults += param.ToString();
        }

        public ICommand SpaceCommand => new DelegateCommands(this.SpaceClick);

        public void SpaceClick(object param)
        {
            this.TypingResults += " ";
        }

        #endregion

        public ViewModel_Main()
        {
            this.Model = new Model_Keys_Localization();
            this.Model.CreateKeys();
            this.TypingResults = "This is a test!";
        }
    }
}
