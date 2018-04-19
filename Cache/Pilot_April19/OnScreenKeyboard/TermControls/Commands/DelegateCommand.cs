namespace TermControls.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// the Delegate command
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// the action.
        /// </summary>
        /// 
        private readonly Action<object> action;

        /// <summary>
        /// initialize a new instance of the <see cref="DelegateCommand"/> class
        /// </summary>
        /// <param name="action">
        /// the action
        /// </param>
        /// 
        public DelegateCommand(Action<object> action)
        {
            this.action = action;
        }

        ///<summary>
        ///the can execute changed
        ///</summary>
        ///
        public event EventHandler CanExecuteChanged
        {
            add
            {

            }
            remove
            {

            }
        }

        ///<summary>
        ///the can execute.
        ///</summary>
        ///<param name="parameter">
        ///the parameter.
        ///</param>
        ///<returns>
        ///The <see cref="bool"/> 
        ///</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

    }
}
