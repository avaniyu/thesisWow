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
        public DeleteModel DeleteModel { get; set; }

        public DeleteViewModel(string _typingResults)
        {
            this.DeleteModel = new DeleteModel(_typingResults);
        }

        public ICommand DeleteCommand => new DelegateCommands(this.DeleteClick);

        public void DeleteClick(object param)
        {
            if (!string.IsNullOrEmpty(this.DeleteModel.TypingResults)) this.DeleteModel.TypingResults = this.DeleteModel.TypingResults.Remove(this.DeleteModel.TypingResults.Length - 1);
        }


    }
}
