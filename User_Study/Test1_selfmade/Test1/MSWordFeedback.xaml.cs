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
    /// Interaction logic for MSWordFeedback.xaml
    /// </summary>
    public partial class MSWordFeedback : Page
    {
        public MSWordFeedback()
        {
            InitializeComponent();
        }

        private void btnNext_click(object sender, RoutedEventArgs e)
        {
            Skype skype = new Skype();
            this.NavigationService.Navigate(skype);
        }
    }
}
