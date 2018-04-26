namespace TermControl.Helpers
{
    using System.Windows;
    using System.Windows.Controls;

    public class GridAutoLayout
    {
        public static readonly DependencyProperty NumberOfColumnsProperty =
            DependencyProperty.RegisterAttached(
                "NumberOfColumns",
                typeof(int),
                typeof(GridAutoLayout),
                new PropertyMetadata(1, NumberOfColumnsUpdated));

        public static readonly DependencyProperty NumberOfRowsProperty =
            DependencyProperty.RegisterAttached(
                "NumberOfRows",
                typeof(int),
                typeof(GridAutoLayout),
                new PropertyMetadata(1, NumberOfRowsUpdated));

        private static void NumberOfColumnsUpdated(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var grid = (Grid)obj;
            grid.ColumnDefinitions.Clear();
            for(var i = 0; i < (int)e.NewValue; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
        }

        private static void NumberOfRowsUpdated(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var grid = (Grid)obj;
            grid.RowDefinitions.Clear();
            for(var i = 0; i < (int)e.NewValue; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
        }

        public static int GetNumberOfColumns(DependencyObject obj)
        {
            return (int)obj.GetValue(NumberOfColumnsProperty);
        }

        public static int GetNumberOfRows(DependencyObject obj)
        {
            return (int)obj.GetValue(NumberOfRowsProperty);
        }

        public static void SetNumberOfRows(DependencyObject obj, int _row)
        {
            obj.SetValue(NumberOfRowsProperty, _row);
        }

        public static void SetNumberOfColumns(DependencyObject obj, int _column)
        {
            obj.SetValue(NumberOfColumnsProperty, _column);
        }
    }
}
