namespace TermControl.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TermControl.Models;
    using TermControl.Commands;
    using System.Windows.Input;

    public class DeleteViewModel
    {
        public DeleteViewModel()
        {
            this.deleteModel = new MainWindowViewModel();
            //this.deleteModel.Model.Text = " ";
        }

        public MainWindowViewModel deleteModel { get; set; }

        public ICommand DeleteCommand => new DelegateCommands(this.DeleteClick);

        public void DeleteClick(object param)
        {
            if (!string.IsNullOrEmpty(this.deleteModel.Model.Text)) this.deleteModel.Model.Text = this.deleteModel.Model.Text.Remove(this.deleteModel.Model.Text.Length - 1);
        }


    }
}
