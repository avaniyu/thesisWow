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

namespace WPF_Pilot_0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ltQ.Click += BtnLetter_Click;   

        }

        private void BtnLetter_Click(object sender, RoutedEventArgs e)
        {
            // sent selected letter to text display
            this.TextDisplay.Text += "Hello world!";
        }

        private void TextDisplay_Read()
        {
            // read typed text from text display
        }
    }
}
