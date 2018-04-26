namespace TermControl.Models
{
    using TermControl.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ComponentModel;
    using System.Collections.ObjectModel;

    public class MainWindowModel:INotifyPropertyChanged
    {
        public MainWindowModel()
        {

        }

        private string typingResults;
        public string TypingResults
        {
            get { return (string)this.typingResults; }
            set
            {
                this.typingResults = value;
                this.onPropertyChanged("TypingResults");
            }
        }


        //public ObservableCollection<KeyboardModel> KeyboardKeys { get; private set; }
        //public ObservableCollection<DeleteModel> DeleteKeys { get; private set; }

        //public void CreateUserControls()
        //{
        //    this.KeyboardKeys = this.CreateKeyboardKeys();
        //    this.DeleteKeys = this.CreateDeleteKeys();
        //}

        //private ObservableCollection<KeyboardModel> CreateKeyboardKeys()
        //{
        //    var keyboardKeys = new ObservableCollection<KeyboardModel>();
        //    keyboardKeys.Add(new KeyboardLanguage { });
        //    return keyboardKeys;
        //}

        //private ObservableCollection<DeleteModel> CreateDeleteKeys()
        //{
        //    var deleteKeys = new ObservableCollection<DeleteModel>();
        //    deleteKeys.Add(new DeleteModel { });
        //    return deleteKeys;
        //}


        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
