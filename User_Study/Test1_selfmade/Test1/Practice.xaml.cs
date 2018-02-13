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

namespace Test1
{
    /// <summary>
    /// Interaction logic for Practice.xaml
    /// </summary>
    public partial class Practice : Page
    {
        public Practice()
        {
            InitializeComponent();
        }

        private void btnConfirmInput_click(object sender, RoutedEventArgs e)
        {
            lbGivenText.Content = "I would try out gaze input keyboard soon!";
            PracticeFeedback practiceFeedback = new PracticeFeedback();
            this.NavigationService.Navigate(practiceFeedback);
        }
    }
}
