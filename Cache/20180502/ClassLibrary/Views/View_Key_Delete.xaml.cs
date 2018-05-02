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
    /// Interaction logic for View_Key_Delete.xaml
    /// </summary>
    public partial class View_Key_Delete : UserControl
    {
        public static readonly DependencyProperty CommandDeleteModeProperty = DependencyProperty.Register(
            "CommandDeleteMode",
            typeof(ICommand),
            typeof(View_Key_Delete));

        public ICommand CommandDeleteMode
        {
            get { return (ICommand)this.GetValue(CommandDeleteModeProperty); }
            set { this.SetValue(CommandDeleteModeProperty, value); }
        }

        public static readonly DependencyProperty CommandDeleteProperty = DependencyProperty.Register(
            "CommandDelete",
            typeof(ICommand),
            typeof(View_Key_Delete));

        public ICommand CommandDelete
        {
            get { return (ICommand)this.GetValue(CommandDeleteProperty); }
            set { this.SetValue(CommandDeleteProperty, value); }
        }

        public View_Key_Delete()
        {
            InitializeComponent();
        }
    }
}
