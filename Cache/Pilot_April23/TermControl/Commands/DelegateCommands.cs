namespace TermControl.Commands
{
    using System;
    using System.Windows.Input;

    public class DelegateCommands : ICommand
    {
        private readonly Action<object> action;
        public DelegateCommands(Action<object> action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
