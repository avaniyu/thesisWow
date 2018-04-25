namespace TermControl.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TermControl.Models;
    using TermControl.Commands;

    public class MainWindowViewModel
    {
        //public MainWindowViewModel()
        //{
        //    this.Model = new KeyboardLanguage();
        //    this.Model.CreateButtons();
        //}

        //public KeyboardModel Model { get; set; }

        public MainWindowModel Model { get; set; }

        public MainWindowViewModel()
        {
            this.Model = new MainWindowModel();
            this.Model.CreateUserControls();
        }
    }
}
