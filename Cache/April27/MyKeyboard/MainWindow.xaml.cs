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

namespace MyKeyboard
{
    //dependency property register, register attached
    //    static is because depedency property syntax
        

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty TypingResultsProperty = DependencyProperty.Register(
            "TypingResults",
            typeof(string),
            typeof(MainWindow),
            new UIPropertyMetadata(null));

        public string TypingResults
        {
            get { return (string)this.GetValue(TypingResultsProperty); }
            set { this.SetValue(TypingResultsProperty, value); }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
