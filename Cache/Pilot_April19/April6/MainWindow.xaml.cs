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

namespace April6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Key_A_Click(object sender, ActivationRoutedEventArgs e)
        {
            typingResults.Text += "A";
        }
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
        private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
