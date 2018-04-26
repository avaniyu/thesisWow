using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace MVVMUserControls
{
    public partial class IconView : UserControl
    {
        IconViewModel _vm;
        public IconView()
        {
            InitializeComponent();
            _vm = (IconViewModel)rootGrid.DataContext;
        }

        public ObservableCollection<IconInfo> IconInfos
        {
            get { return (ObservableCollection<IconInfo>)GetValue(IconInfosProperty); }
            set { SetValue(IconInfosProperty, value); }
        }

        public static readonly DependencyProperty IconInfosProperty =
            DependencyProperty.Register("IconInfos", typeof(ObservableCollection<IconInfo>), 
                typeof(IconView), new PropertyMetadata(null, OnIconInfosSet));

        private static void OnIconInfosSet(DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ((IconView)d)._vm.IconInfos = e.NewValue as ObservableCollection<IconInfo>;
        }

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), 
                typeof(IconView), new PropertyMetadata(-1.0));

        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }

        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.Register("IconMargin", typeof(Thickness), 
                typeof(IconView), new PropertyMetadata(new Thickness(0)));

    }
}
