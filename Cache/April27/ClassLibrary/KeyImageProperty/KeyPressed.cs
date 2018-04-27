namespace ClassLibrary.KeyImageProperty
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;

    public class KeyPressed
    {
        public static readonly DependencyProperty ImageProperty;
        static KeyPressed()
        {
            var metadata = new FrameworkPropertyMetadata((ImageSource)null);
            ImageProperty = DependencyProperty.RegisterAttached(
                "Image",
                typeof(ImageSource),
                typeof(KeyPressed),
                metadata);
        }

        public static ImageSource GetImage(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(ImageProperty);
        }

        public static void SetImage(DependencyObject obj, ImageSource img)
        {
            obj.SetValue(ImageProperty, img);
        }
    }
}
