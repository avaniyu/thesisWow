namespace TermControl.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TermControl.Models;

    public class KeyboardModel : INotifyPropertyChanged
    {
        protected string[] Content1 { get; set; }
        protected string[] Content1Shift { get; set; }
        protected string[] Content2 { get; set; }
        protected string[] Content2Shift { get; set; }

        public bool IsShift { get; set; }
        public bool IsEngOther { get; set; }

        public string[] Content
        {
            get
            {
                if (!this.IsShift && this.IsEngOther) return Content1;
                if (this.IsShift && this.IsEngOther) return Content1Shift;
                if (!this.IsShift && !this.IsEngOther) return Content2;
                if (this.IsShift && !this.IsEngOther) return Content2Shift;
                return null;                
            }
        }

        public ObservableCollection<ButtonModel> ButtonsRow1 { get; private set; }
        public ObservableCollection<ButtonModel> ButtonsRow2 { get; private set; }
        public ObservableCollection<ButtonModel> ButtonsRow3 { get; private set; }

        //private string text;
        //public string Text
        //{
        //    get { return this.text; }
        //    set
        //    {
        //        this.text = value;
        //        this.onPropertyChanged("Text");
        //    }
        //}

        //public string TypingResults
        //{
        //    get { return (string)this.typingResults; }
        //    set
        //    {
        //        this.typingResults = value;
        //        this.onPropertyChanged("TypingResults");
        //    }
        //}

        #region methods
        public string GetButtonContent(string btnName)
        {
            var row = Convert.ToInt32(btnName[1].ToString()) - 1;
            var col = btnName.Length == 3
                ? Convert.ToInt32(btnName[2].ToString()) - 1
                : Convert.ToInt32(btnName[2] + btnName[3].ToString()) - 1;
            return this.Content[row][col].ToString();
        }

        public void ChangeButtonsContent()
        {
            this.ChangeButtonsContent(this.ButtonsRow1, 0);
            this.ChangeButtonsContent(this.ButtonsRow2, 1);
            this.ChangeButtonsContent(this.ButtonsRow3, 2);
        }

        public void CreateButtons()
        {
            this.ButtonsRow1 = this.CreateButtons(0);
            this.ButtonsRow2 = this.CreateButtons(1);
            this.ButtonsRow3 = this.CreateButtons(2);
        }

        public virtual void InitContent() { }
        
        public KeyboardModel()
        {
            this.IsShift = false;
            this.IsEngOther = false;
            this.InitContent();
        }

        private void ChangeButtonsContent(ObservableCollection<ButtonModel> buttons, int _row)
        {
            for(var j = 1; j < this.Content[_row].Length; j++)
            {
                buttons[j - 1].Content = this.GetButtonContent(buttons[j - 1].Name); //????
            }
        }

        private ObservableCollection<ButtonModel> CreateButtons(int row)
        {
            var buttons = new ObservableCollection<ButtonModel>();
            for (var j = 1; j < this.Content[row].Length; j++)
            {
                var name = $"b{row + 1}{j}";
                buttons.Add(new ButtonModel { Name = name, Column = j - 1, Content = this.GetButtonContent(name) });
            }
            return buttons;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
