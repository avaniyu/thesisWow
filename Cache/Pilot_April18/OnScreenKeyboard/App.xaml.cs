using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OnScreenKeyboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private Host _host;
        //private WpfInteractorAgent _agent;

        protected override void OnStartup(StartupEventArgs e)
        {
            //_host = new Host();
            //_agent = _host.InitializeWpfAgent();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //_host.DisableConnection();
            base.OnExit(e);
        }

    }
}
