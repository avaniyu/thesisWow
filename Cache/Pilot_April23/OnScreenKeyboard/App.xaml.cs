using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tobii.Interaction;
using Tobii.Interaction.Wpf;

namespace OnScreenKeyboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Host _host;
        private WpfInteractorAgent _agent;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _host = new Host();
            _agent = _host.InitializeWpfAgent();

        }
    }
}
