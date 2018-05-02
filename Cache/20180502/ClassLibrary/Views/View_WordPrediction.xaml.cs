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
    /// Interaction logic for View_WordPrediction.xaml
    /// </summary>
    public partial class View_WordPrediction : UserControl
    {
        public static readonly DependencyProperty CommandTestProperty = DependencyProperty.Register(
            "CommandTest",
            typeof(ICommand),
            typeof(View_WordPrediction));

        public ICommand CommandTest
        {
            get { return (ICommand)this.GetValue(CommandTestProperty); }
            set { this.SetValue(CommandTestProperty, value); }
        }

        public View_WordPrediction()
        {
            InitializeComponent();
        }
    }
}
