﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test1
{
    /// <summary>
    /// Interaction logic for SkypeFeedback.xaml
    /// </summary>
    public partial class SkypeFeedback : Page
    {
        public SkypeFeedback()
        {
            InitializeComponent();
        }

        private void btnNext_click(object sender, RoutedEventArgs e)
        {
            SubjectiveRating subjectiveRating = new SubjectiveRating();
            this.NavigationService.Navigate(subjectiveRating);
        }
    } 
}
