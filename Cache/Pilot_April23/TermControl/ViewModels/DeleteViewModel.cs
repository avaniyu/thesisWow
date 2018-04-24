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

        }

        public ICommand DeleteCommand => new DelegateCommands(DeleteClick);
        private void DeleteClick(object param)
        {
            this.
        }
    }
}
