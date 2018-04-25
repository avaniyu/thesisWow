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

        private string text;
        public string Text
        {
            get { return (string)this.text; }
            set
            {
                this.text = value;
                this.onPropertyChanged("Text");
            }
        }

        //how to bind this.text and .TypingResults from the following ObservableCollection<>?

        public ObservableCollection<KeyboardModel> KeyboardKeys { get; private set; }
        public ObservableCollection<DeleteModel> DeleteKeys { get; private set; }
        public ObservableCollection<WordPredictionModel> WordPredictors { get; private set; }

        public void CreateUserControls()
        {
            this.KeyboardKeys = this.CreateKeyboardKeys();
            this.DeleteKeys = this.CreateDeleteKeys();
            this.WordPredictors = this.CreateWordPredictors();
        }

        private ObservableCollection<KeyboardModel> CreateKeyboardKeys()
        {
            var keyboardKeys = new ObservableCollection<KeyboardModel>();
            keyboardKeys.Add(new KeyboardLanguage { });
            return keyboardKeys;
        }

        private ObservableCollection<DeleteModel> CreateDeleteKeys()
        {
            var deleteKeys = new ObservableCollection<DeleteModel>();
            deleteKeys.Add(new DeleteModel { });
            return deleteKeys;
        }

        private ObservableCollection<WordPredictionModel> CreateWordPredictors()
        {
            var wordPredictors = new ObservableCollection<WordPredictionModel>();
            wordPredictors.Add(new WordPredictionModel { });
            return wordPredictors;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
