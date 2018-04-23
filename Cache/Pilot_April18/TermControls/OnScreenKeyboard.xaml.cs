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

namespace TermControls
{
    /// <summary>
    /// Interaction logic for OnScreenKeyboard.xaml
    /// </summary>
    public partial class OnScreenKeyboard : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(OnScreenKeyboard),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(string),
            typeof(OnScreenKeyboard));

        public static readonly DependencyProperty NumberOfColumnsProperty =
             DependencyProperty.Register(
                 "NumberOfColumns",
                 typeof(int),
                 typeof(OnScreenKeyboard),
                 new PropertyMetadata(1));

        public static readonly DependencyProperty NumberOfRowsProperty =
            DependencyProperty.Register(
                "NumberOfRows",
                typeof(int),
                typeof(OnScreenKeyboard),
                new PropertyMetadata(1));


        public OnScreenKeyboard()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }


        public int NumberOfColumns
        {
            get { return (int)this.GetValue(NumberOfColumnsProperty); }
            set { this.SetValue(NumberOfColumnsProperty, value); }
        }


        public int NumberOfRows
        {
            get { return (int)this.GetValue(NumberOfRowsProperty); }
            set { this.SetValue(NumberOfRowsProperty, value); }
        }


    }
}
