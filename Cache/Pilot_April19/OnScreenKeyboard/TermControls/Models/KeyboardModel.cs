namespace TermControls.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class KeyboardModel : INotifyPropertyChanged
    {
        protected string[] Content1 { get; set; }
        protected string[] Content1Shift { get; set; }
        
        protected string[] Content2 { get; set; }
        protected string[] Content2Shift { get; set; }

        public bool IsShift { get; set; }

        public bool IsEngRus { get; set; }

        public string[] Content
        {
            get
            {
                if (!this.IsShift && this.IsEngRus) return this.Content1;
                if (this.IsShift && this.IsEngRus) return this.Content1Shift;
                if (!this.IsShift && !this.IsEngRus) return this.Content2;
                if (this.IsShift && !this.IsEngRus) return this.Content2Shift;
                return null;
            }
        }

        public ObservableCollection<ButtonModel> ButtonRaw1 { get; private set; }
        public ObservableCollection<ButtonModel> ButtonRaw2 { get; private set; }
        public ObservableCollection<ButtonModel> ButtonRaw3 { get; private set; }
        public ObservableCollection<ButtonModel> ButtonRaw4 { get; private set; }

        private string text;
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.OnPropertyChanged("Text");
            }
        }

        public string Text2 { get; set; }

        #region Methods
        public string GetButtonContent(string btnName)
        {
            var raw = Convert.ToInt32(btnName[1].ToString()) - 1;
            var col = btnName.Length == 3
                ? Convert.ToInt32(btnName[2].ToString()) - 1
                : Convert.ToInt32(btnName[2] + btnName[3].ToString()) - 1;
            return this.Content[raw][col].ToString();
        }

        public void ChangeButtonsContent()
        {
            this.ChangeButtonsContent(this.ButtonRaw1, 0);
            this.ChangeButtonsContent(this.ButtonRaw2, 1);
            this.ChangeButtonsContent(this.ButtonRaw3, 2);
            this.ChangeButtonsContent(this.ButtonRaw4, 3);
        }

        public void CreateButton()
        {
            this.ButtonRaw1 = this.CreateButton(0);
            this.ButtonRaw1 = this.CreateButton(1);
            this.ButtonRaw1 = this.CreateButton(2);
            this.ButtonRaw1 = this.CreateButton(3);
        }

        public KeyboardModel()
        {
            this.IsShift = false;
            this.IsEngRus = false;
            this.InitContent();
        }

        public virtual void InitContent() { }

        private ObservableCollection<ButtonModel> CreateButtons(int raw)
        {
            var buttons = new ObservableCollection<ButtonModel>();

        }
    }
}
