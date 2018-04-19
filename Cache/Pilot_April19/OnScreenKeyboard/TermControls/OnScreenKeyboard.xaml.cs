using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TermControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for OnScreenKeyboard.xaml
    /// </summary>
    public partial class OnScreenKeyboard : UserControl
    {
        /// <summary>
        /// the on screen keyboard.
        /// </summary>
        /// 
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(OnScreenKeyboard),
            new UIPropertyMetadata(null));

        ///<summary>
        ///the command property
        ///</summary>
        ///
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(OnScreenKeyboard));

        ///<summary>
        ///initialize a new instance of the <see cref="OnScreenKeyboard"/>
        ///</summary>
        ///
        public OnScreenKeyboard()
        {
            this.InitializeComponent();
        }

        ///<summary>
        ///gets or sets the text
        ///</summary>
        ///
        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }
            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        ///<summary>
        ///gets or sets the command
        ///</summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }
            set
            {
                this.SetValue(CommandProperty, value);
            }
        }
    }
}
