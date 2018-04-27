namespace ClassLibrary.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using ClassLibrary.Helpers;

    public class Model_Keys : Base_Binding
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
                if (!this.IsShift && this.IsEngOther) return this.Content1;
                if (this.IsShift && this.IsEngOther) return this.Content1Shift;
                if (!this.IsShift && !this.IsEngOther) return this.Content2;
                if (this.IsShift && !this.IsEngOther) return this.Content2Shift;
                return null;
            }
        }

        public ObservableCollection<Model_Key> KeysRow1 { get; private set; }
        public ObservableCollection<Model_Key> KeysRow2 { get; private set; }
        public ObservableCollection<Model_Key> KeysRow3 { get; private set; }

        private string typingResults;
        public string TypingResults
        {
            get { return this.typingResults; }
            set
            {
                this.typingResults = value;
                this.OnPropertyChanged(nameof(TypingResults));
            }
        }

        #region Methods
        public string GetKeyContent(string btnName)
        {
            var row = Convert.ToInt32(btnName[1].ToString()) - 1;
            var col = btnName.Length == 3
                          ? Convert.ToInt32(btnName[2].ToString()) - 1
                          : Convert.ToInt32(btnName[2] + btnName[3].ToString()) - 1;
            return this.Content[row][col].ToString();
        }

        public void ChangeKeysContent()
        {
            this.ChangeKeysContent(this.KeysRow1, 0);
            this.ChangeKeysContent(this.KeysRow2, 1);
            this.ChangeKeysContent(this.KeysRow3, 2);
        }

        public void CreateButtons()
        {
            this.KeysRow1 = this.CreateKeys(0);
            this.KeysRow2 = this.CreateKeys(1);
            this.KeysRow3 = this.CreateKeys(2);
        }

        public Model_Keys()
        {
            this.IsShift = false;
            this.IsEngOther = false;
            this.InitContent();
        }

        public virtual void InitContent()
        {
        }

        private ObservableCollection<Model_Key> CreateKeys(int Row)
        {
            var keys = new ObservableCollection<Model_Key>();
            for (var j = 1; j <= this.Content[Row].Length; j++)
            {
                var name = $"b{Row + 1}{j}";
                keys.Add(new Model_Key { Name = name, Column = j - 1, Content = this.GetKeyContent(name) });
            }
            return keys;
        }

        private void ChangeKeysContent(ObservableCollection<Model_Key> Keys, int row)
        {
            for (var j = 1; j <= this.Content[row].Length; j++) Keys[j - 1].Content = this.GetKeyContent(Keys[j - 1].Name);
        }

        #endregion
    }
}
