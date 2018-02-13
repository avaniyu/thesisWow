using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using Tobii.Interaction;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Interaction_Wpf_101
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int indexGivenTxt = 0;
        List<string> lstGivenTxts = new List<string>();
        Stopwatch swWpm = Stopwatch.StartNew();

        public MainWindow()
        {
            InitializeComponent();

            var host = new Host();
            var currentWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
            var currentWindowBounds = GetWindowBounds(currentWindowHandle);
            var interactorAgent = host.InitializeVirtualInteractorAgent(currentWindowHandle, "UserStudyAppAgent");
            interactorAgent
                .AddInteractorFor(currentWindowBounds)
                .WithGazeAware()
                .HasGaze(() => Console.WriteLine("Hey there!"))
                .LostGaze(() => Console.WriteLine("Bye..."));

            // read given text from file
            lstGivenTxts.Add("Hello! I am Jiayao.");
            lstGivenTxts.Add("I miss the April in Jia Xing.");
            lstGivenTxts.Add("I'd go home to see.");
            //delete existing file
            if (File.Exists(@"C:\Users\jiyu\Jiayao\GitRepo\Gaze_Keyboard\User_Study\samples\timeInterval.txt"))
                File.Delete(@"C:\Users\jiyu\Jiayao\GitRepo\Gaze_Keyboard\User_Study\samples\timeInterval.txt");
        }



        private void TypingResults_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TxtBoxInput.Text.Length == 1)
                swWpm.Restart();
        }

        private void GazeConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (indexGivenTxt < lstGivenTxts.Count)
            {
                // compute wpm
                swWpm.Stop();
                //record final typing results for a sentence
                string strUserFinalInput = TxtBoxInput.Text;
                int countWords = 0, index = 0;
                while (index < strUserFinalInput.Length)
                {
                    if (char.IsWhiteSpace(strUserFinalInput[index]))
                        countWords++; //count word is incorrect
                    index++;
                }
                TimeSpan tsSentence = swWpm.Elapsed;
                int timeSentence = tsSentence.Milliseconds + tsSentence.Seconds * 1000 + tsSentence.Minutes * 60000;
                float wpm = 60000 / (timeSentence / countWords);

                using (StreamWriter file = new StreamWriter(@"C:\Users\jiyu\Jiayao\GitRepo\Gaze_Keyboard\User_Study\samples\timeInterval.txt", true))
                {
                    file.WriteLine("index: " + indexGivenTxt + " timeSentence: " + timeSentence + " wordCount: " + countWords + " wpm: " + wpm);
                }

                //change given text from read file
                TxtBoxGiven.Text = lstGivenTxts[indexGivenTxt];
                indexGivenTxt++;

                // clear TxtBoxInput
                TxtBoxInput.Text = "";
            }
            else
            {
                //close the host, but dk how to do that though
            }
        }

        #region Helpers
        private static Rectangle GetWindowBounds(IntPtr windowHandle)
        {
            NativeRect nativeNativeRect;
            if (GetWindowRect(windowHandle, out nativeNativeRect))
                return new Rectangle
                {
                    X = nativeNativeRect.Left,
                    Y = nativeNativeRect.Top,
                    Width = nativeNativeRect.Right,
                    Height = nativeNativeRect.Bottom
                };
            return new Rectangle(0d, 0d, 1000d, 1000d); 
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out NativeRect nativeRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion
    }
}
