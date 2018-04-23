namespace TermControls.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class KeyboardModel:INotifyPropertyChanged
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
                if (!this.IsShift && this.IsEngRus) return Content1;
                if (this.IsShift && this.IsEngRus) return Content1Shift;
                if (!this.IsShift && !this.IsEngRus) return Content2;
                if (this.IsShift && !this.IsEngRus) return Content2Shift;
                return null;
            }
        }

        public ObservableCollection<ButtonModel> ButtonsRaw1 { get; private set; }
        public ObservableCollection<ButtonModel> ButtonsRaw2 { get; private set; }
        public ObservableCollection<ButtonModel> ButtonsRaw3 { get; private set; }
        //public ObservableCollection<ButtonModel> ButtonsRaw4 { get; private set; }

        private string text;
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                this.OnPropertyChanged("Text");
            }
        }

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
            this.ChangeButtonsContent(this.ButtonsRaw1, 0);
            this.ChangeButtonsContent(this.ButtonsRaw2, 1);
            this.ChangeButtonsContent(this.ButtonsRaw3, 2);
            //this.ChangeButtonsContent(this.ButtonsRaw4, 3);
        }

        public void CreateButtons()
        {
            this.ButtonsRaw1 = this.CreateButtons(0);
            this.ButtonsRaw2 = this.CreateButtons(1);
            this.ButtonsRaw3 = this.CreateButtons(2);
            //this.ButtonsRaw4 = this.CreateButtons(3);
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
            for(var j = 1; j < this.Content[raw].Length; j++)
            {
                var name = $"b{raw + 1}{j}";
                buttons.Add(new ButtonModel { Name = name, Column = j - 1, Content = this.GetButtonContent(name) });
            }
            return buttons;
        }

        private void ChangeButtonsContent(ObservableCollection<ButtonModel> buttons, int _raw)
        {
            for (var j = 1; j <= this.Content[_raw].Length; j++) buttons[j - 1].Content = this.GetButtonContent(buttons[j - 1].Name);
        }

        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
