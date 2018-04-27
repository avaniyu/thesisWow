namespace ClassLibrary.KeyImageProperty
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;

    public class KeyNotPressed
    {
        public static readonly DependencyProperty ImageProperty;
        static KeyNotPressed()
        {
            var metadata = new FrameworkPropertyMetadata((ImageSource)null);
            ImageProperty = DependencyProperty.RegisterAttached(
                "Image",
                typeof(ImageSource),
                typeof(KeyNotPressed),
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
        
        //propdp


    }
}
