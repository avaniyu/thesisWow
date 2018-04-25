using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Tobii.Interaction;
using Tobii.Interaction.Wpf;

namespace TermControl
{
    /// <summary>
    /// Interaction logic for OnScreenKeyboard.xaml
    /// </summary>
    public partial class OnScreenKeyboard : UserControl
    {
        private Host _host;
        private WpfInteractorAgent _agent;

        public OnScreenKeyboard()
        {
            InitializeComponent();
            _host = new Host();
            _agent = _host.InitializeWpfAgent();
        }
    }
}
