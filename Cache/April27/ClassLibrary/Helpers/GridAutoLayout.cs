namespace ClassLibrary.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    public class GridAutoLayout
    {
        public static readonly DependencyProperty NumberOfColumnsProperty = DependencyProperty.RegisterAttached(
            "NumberOfColumns",
            typeof(int),
            typeof(GridAutoLayout),
            new PropertyMetadata(1, NumbeOfColumnsUpdated));

        public static readonly DependencyProperty NumberOfRowsProperty = DependencyProperty.RegisterAttached(
            "NumberOfRows",
            typeof(int),
            typeof(GridAutoLayout),
            new PropertyMetadata(1, NumberOfRowsUpdated));

        private static void NumbeOfColumnsUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (Grid)d;
            grid.ColumnDefinitions.Clear();
            for (int i = 0; i < (int)e.NewValue; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
        }

        private static void NumberOfRowsUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (Grid)d;
            grid.RowDefinitions.Clear();
            for(int i = 0; i < (int)e.NewValue; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
        }

        public static void SetNumberOfColumns(DependencyObject d, int col)
        {
            d.SetValue(NumberOfColumnsProperty, col);
        }

        public static void SetNumberOfRows(DependencyObject d, int row)
        {
            d.SetValue(NumberOfRowsProperty, row);
        }

        public static int GetNumberOfColumns(DependencyObject d)
        {
            return (int)d.GetValue(NumberOfColumnsProperty);
        }

        public static int GetNumberOfRows(DependencyObject d)
        {
            return (int)d.GetValue(NumberOfRowsProperty);
        }

    }
}
