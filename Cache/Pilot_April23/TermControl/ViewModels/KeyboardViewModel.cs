namespace TermControl.ViewModels
{
    using TermControl.Models;
    using TermControl.Commands;
    using TermControl.ViewModels;
    using System.Windows.Input;

    public class KeyboardViewModel
    {
        public KeyboardLanguage KeyboardKeysModel { get; set; }

        public KeyboardViewModel()
        {
            this.KeyboardKeysModel = new KeyboardLanguage();
            this.KeyboardKeysModel.CreateButtons();
        }



        //public MainWindowViewModel KeyboardKeysModel { get; set; }

        //public ICommand ChangeLangCommand
        //{
        //    get
        //    {
        //        return new DelegateCommands(this.ChangeLangClick);
        //    }
        //}

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
            this.KeyboardKeysModel.TypingResults += param.ToString();
        }

        public ICommand SpaceCommand => new DelegateCommands(this.SpaceClick);

        public void SpaceClick(object param)
        {
            //this.KeyboardKeysModel.Text += " ";
            this.KeyboardKeysModel.TypingResults += " ";
        }
    }
}
