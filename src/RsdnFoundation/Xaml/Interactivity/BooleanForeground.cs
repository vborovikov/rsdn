namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Documents;
    using Windows.UI.Xaml.Media;

    public static class BooleanForeground
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(bool), typeof(BooleanForeground),
                new PropertyMetadata(false, HandlePropertyChanged));

        public static readonly DependencyProperty TrueForegroundProperty =
            DependencyProperty.RegisterAttached("TrueForeground", typeof(Brush), typeof(BooleanForeground),
                new PropertyMetadata(null, HandlePropertyChanged));

        public static readonly DependencyProperty FalseForegroundProperty =
            DependencyProperty.RegisterAttached("FalseForeground", typeof(Brush), typeof(BooleanForeground),
                new PropertyMetadata(null, HandlePropertyChanged));

        public static bool GetValue(DependencyObject obj)
        {
            return (bool)obj.GetValue(ValueProperty);
        }

        public static void SetValue(DependencyObject obj, bool value)
        {
            obj.SetValue(ValueProperty, value);
        }

        public static Brush GetTrueForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(TrueForegroundProperty);
        }

        public static void SetTrueForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(TrueForegroundProperty, value);
        }

        public static Brush GetFalseForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FalseForegroundProperty);
        }

        public static void SetFalseForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(FalseForegroundProperty, value);
        }

        private static void HandlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = GetValue(d);
            ChangeForeground(d, value);
        }

        private static void ChangeForeground(DependencyObject obj, bool flag)
        {
            var foreground = flag ? GetTrueForeground(obj) : GetFalseForeground(obj);

            var textElement = obj as TextElement;
            if (textElement != null)
            {
                textElement.Foreground = foreground;
                return;
            }

            var textBlock = obj as TextBlock;
            if (textBlock != null)
            {
                textBlock.Foreground = foreground;
                return;
            }

            var richTextBlock = obj as RichTextBlock;
            if (richTextBlock != null)
            {
                richTextBlock.Foreground = foreground;
                return;
            }

            var control = obj as Control;
            if (control != null)
            {
                control.Foreground = foreground;
                return;
            }
        }
    }
}