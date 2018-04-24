namespace TermControl.ViewModels
{
    using TermControl.Models;
    using TermControl.Commands;
    using System.Windows.Input;

    public class KeyboardViewModel
    {
        public KeyboardViewModel()
        {
            this.Model = new KeyboardLanguage();
            this.Model.CreateButtons();
        }

        public KeyboardModel Model { get; set; }

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
            this.Model.IsEngOther = !this.Model.IsEngOther;
            this.Model.ChangeButtonsContent();
        }

        public ICommand ShiftCommand => new DelegateCommands(this.ShiftClick);

        public void ShiftClick(object param)
        {
            this.Model.IsShift = !this.Model.IsShift;
            this.Model.ChangeButtonsContent();
        }

        public ICommand DeleteCommand => new DelegateCommands(this.DeleteClick);

        public void DeleteClick(object param)
        {
            if (!string.IsNullOrEmpty(this.Model.Text)) this.Model.Text = this.Model.Text.Remove(this.Model.Text.Length - 1);
        }

        public ICommand ButtonClickCommand => new DelegateCommands(this.ButtonClick);

        public void ButtonClick(object param)
        {
            this.Model.Text += param.ToString();
        }

        public ICommand SpaceCommand => new DelegateCommands(this.SpaceClick);

        public void SpaceClick(object param)
        {
            this.Model.Text += " ";
        }
    }
}
