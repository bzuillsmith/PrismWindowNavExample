using System.Windows;

namespace PrismWindowNavExample.Navigation
{
    public class WindowProps : DependencyObject
    {
        public static readonly DependencyProperty DefaultWidthProperty = DependencyProperty.RegisterAttached(
                "DefaultWidth",
                typeof(double),
                typeof(WindowProps),
                new FrameworkPropertyMetadata(0d)
        );
        public static void SetDefaultWidth(UIElement element, double value)
        {
            element.SetValue(DefaultWidthProperty, value);
        }
        public static double GetDefaultWidth(UIElement element)
        {
            return (double)element.GetValue(DefaultWidthProperty);
        }

        public static readonly DependencyProperty DefaultHeightProperty = DependencyProperty.RegisterAttached(
                "DefaultHeight",
                typeof(double),
                typeof(WindowProps),
                new FrameworkPropertyMetadata(0d)
        );
        public static void SetDefaultHeight(UIElement element, double value)
        {
            element.SetValue(DefaultHeightProperty, value);
        }
        public static double GetDefaultHeight(UIElement element)
        {
            return (double)element.GetValue(DefaultHeightProperty);
        }
    }
}
