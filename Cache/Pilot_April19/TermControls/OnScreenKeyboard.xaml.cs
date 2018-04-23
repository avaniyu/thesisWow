namespace TermControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Tobii.Interaction;
    using Tobii.Interaction.Wpf;

    /// <summary>
    ///     The on screen keyboard.
    /// </summary>
    public partial class OnScreenKeyboard : UserControl
    {
        // for enabling tobii core sdk
        private Host _host;
        private WpfInteractorAgent _agent;

        /// <summary>
        ///     The text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(OnScreenKeyboard),
            new UIPropertyMetadata(null));

        /// <summary>
        ///     The command property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(OnScreenKeyboard));

        /// <summary>
        /// Initializes a new instance of the <see cref="OnScreenKeyboard"/> class. 
        /// </summary>
        public OnScreenKeyboard()
        {
            this.InitializeComponent();
            _host = new Host();
            _agent = _host.InitializeWpfAgent();
        }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
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

        /// <summary>
        ///     Gets or sets the command.
        /// </summary>
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
