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

namespace ClassLibrary.Views
{
    /// <summary>
    /// Interaction logic for View_TypingResults.xaml
    /// </summary>
    public partial class View_TypingResults : UserControl
    {
        public static readonly DependencyProperty CommandConfirmProperty = DependencyProperty.Register(
            "CommandConfirm",
            typeof(ICommand),
            typeof(View_TypingResults));

        public ICommand CommandConfirm
        {
            get { return (ICommand)this.GetValue(CommandConfirmProperty); }
            set { this.SetValue(CommandConfirmProperty, value); }
        }

        public View_TypingResults()
        {
            InitializeComponent();
        }
    }
}
