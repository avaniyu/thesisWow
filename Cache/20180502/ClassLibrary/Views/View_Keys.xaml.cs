namespace ClassLibrary.Views
{
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

    /// <summary>
    /// Interaction logic for View_Keys.xaml
    /// </summary>
    public partial class View_Keys : UserControl
    {
        //public static readonly DependencyProperty TypingResultsProperty = DependencyProperty.Register(
        //    "TypingResults",
        //    typeof(string),
        //    typeof(View_Keys),
        //    new UIPropertyMetadata(null));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(View_Keys));

        public View_Keys()
        {
            InitializeComponent();
        }

        //public string TypingResults
        //{
        //    get { return (string)this.GetValue(TypingResultsProperty); }
        //    set { this.SetValue(TypingResultsProperty, value); }
        //}

        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

    }
}
