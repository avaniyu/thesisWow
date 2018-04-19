using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermControls.Helpers
{
    using System.Windows;
    using System.Windows.Controls;

    public class GridAutoLayout
    {
        /// <summary>
        /// the number of columns property
        /// </summary>
        public static readonly DependencyProperty NumberOfColumnsProperty =
            DependencyProperty.RegisterAttached(
                "NumberOfColumns",
                typeof(int),
                typeof(GridAutoLayout),
                new PropertyMetadata(1, NumberOfColumnsUpdated));

        /// <summary>
        /// the number of rows property
        /// </summary>
        /// 
        public static readonly DependencyProperty NumberOfRowsProperty =
            DependencyProperty.RegisterAttached(
                "NumberOfRows",
                typeof(int),
                typeof(GridAutoLayout),
                new PropertyMetadata(1, NumberOfRowsUpdated));

        /// <summary>
        /// the get number of columns
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// the <see cref="int"/>
        /// </returns>
        /// 
        public static int GetNumberOfColumns(DependencyObject obj)
        {
            return (int)obj.GetValue(NumberOfColumnsProperty);
        }

        /// <summary>
        /// the set number of columns
        /// </summary>
        /// <param name="obj">
        /// the object.
        /// </param>
        /// <param name="value">
        /// the value
        /// </param>
        /// 
        public static void SetNumberOfColumns(DependencyObject obj, int value)
        {
            obj.SetValue(NumberOfColumnsProperty, value);
        }

        private static void NumberOfColumnsUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (Grid)d;
            grid.ColumnDefinitions.Clear();
            for (var i = 0; i< (int)e.NewValue; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
        }

        public static int GetNumberOfRows(DependencyObject obj)
        {
            return (int)obj.GetValue(NumberOfRowsProperty);
        }

        public static void SetNumberOfRows(DependencyObject obj, int value)
        {
            obj.SetValue(NumberOfRowsProperty, value);
        }

        private static void NumberOfRowsUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = (Grid)d;
            grid.RowDefinitions.Clear();
            for (var i = 0; i < (int)e.NewValue; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
        }

    }
}
