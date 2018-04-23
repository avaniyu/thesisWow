namespace TermControls.Models
{
    /// <summary>
    /// The keyboard model russian english.
    /// </summary>
    public class KeyboardModelEngOther : KeyboardModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardModelEngOther"/> class.
        /// </summary>
        public KeyboardModelEngOther()
            : base()
        {
        }

        /// <summary>
        /// The initialize content.
        /// </summary>
        public override void InitContent()
        {
            this.Content1 = new[] { "1234567890-", "qwertyuiop[]", "asdfghjkl;'", "zxcvbnm<>,.?" };
            this.Content1Shift = new[] { "!@#$%^&*()-", "QWERTYUIOP{}", "ASDFGHJKL:\"", "ZXCVBNM<>,.?" };
            //another language
            this.Content2 = new[] { "1234567890-", "qwertyuiop[]", "asdfghjkl;'", "zxcvbnm<>,.?" };
            this.Content2Shift = new[] { "!@#$%^&*()-", "QWERTYUIOP{}", "ASDFGHJKL:\"", "ZXCVBNM<>,.?" };
        }
    }
}
