namespace ClassLibrary.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ClassLibrary.Models;
    using ClassLibrary.Commands;
    using System.ComponentModel;
    using System.Windows.Input;
    using ClassLibrary.Helpers;

    public class ViewModel_Main : Base_Binding
    {
        private string typingResults;
        public string TypingResults
        {
            get { return (string)this.typingResults; }
            set
            {
                typingResults = value;
                OnPropertyChanged(nameof(TypingResults));
            }
        }

        private string typingBuffer;
        public bool isWordDeletion { get; set; }

        public Model_Keys_Localization Model { get; set; }

        #region for keyboard keys

        //public ICommand ChangeLangCommand => new DelegateCommands(this.ChangeLangClick);

        //public void ChangeLangClick(object param)
        //{
        //    this.Model.IsEngOther = !this.Model.IsEngOther;
        //    this.Model.ChangeKeysContent();
        //}

        //public ICommand ShiftCommand => new DelegateCommands(this.ShiftClick);

        //public void ShiftClick(object param)
        //{
        //    this.Model.IsShift = !this.Model.IsShift;
        //    this.Model.ChangeKeysContent();
        //}

        public ICommand KeyClickCommand => new DelegateCommands(this.KeyClick);

        public void KeyClick(object param)
        {
            this.typingBuffer += param.ToString();
        }

        //public ICommand SpaceCommand => new DelegateCommands(this.SpaceClick);

        //public void SpaceClick(object param)
        //{
        //    this.TypingResults += " ";
        //}

        #endregion

        #region for typing results display
        public ICommand ConfirmCommand => new DelegateCommands(this.ConfirmClick);
        
        public void ConfirmClick(object param)
        {
            this.TypingResults += this.typingBuffer;
            this.typingBuffer = "";
        }

        #endregion

        #region for Deletion

        public ICommand DeleteCommand => new DelegateCommands(this.DeleteClick);
        public void DeleteClick(object param)
        {
            if (this.isWordDeletion == true)
            {
                if (!string.IsNullOrEmpty(this.TypingResults))
                {
                    this.TypingResults = this.TypingResults.Remove(this.TypingResults.Length - 1);
                }
            }
            else
            {
                this.TypingResults += "<Delete Mode 2>";
            }
        }

        public ICommand DeleteModeCommand => new DelegateCommands(this.DeleteModeClick);
        public void DeleteModeClick(object param)
        {
            this.isWordDeletion = !this.isWordDeletion;
        }

        #endregion

        #region for predictors
        public ICommand TestCommand => new DelegateCommands(TestClick);
        public void TestClick(object param)
        {
            //this.TypingResults = "Test!";
            var helper = new GetSokgraph("typing");
            var sokgraph = helper.WordSokgraph;
            var baseCoord = helper.TestSampleBaseLettersCoordinates;
            for(int i = 0; i < sokgraph.Count; i++)
            {
                for(int j = 0; j < sokgraph[i].Count; j++)
                {
                    this.TypingResults += sokgraph[i][j].ToString();
                    this.TypingResults += ", ";
                }
                this.TypingResults += " | ";
            }

            this.TypingResults += "\n";

            for (int i = 0; i < baseCoord.Count; i++)
            {
                for (int j = 0; j < baseCoord[i].Count; j++)
                {
                    this.TypingResults += baseCoord[i][j].ToString();
                    this.TypingResults += ", ";
                }
                this.TypingResults += " | ";
            }
        }
        #endregion


        public ViewModel_Main()
        {
            this.Model = new Model_Keys_Localization();
            this.Model.CreateKeys();
            this.TypingResults = "";
            this.typingBuffer = "";
            this.isWordDeletion = true;
        }
    }
}
