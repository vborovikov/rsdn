namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Input;

    public static class Flyout
    {
        // Using a DependencyProperty as the backing store for ShowOnTap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowOnTapProperty =
            DependencyProperty.RegisterAttached("ShowOnTap", typeof(bool),
                typeof(Flyout), new PropertyMetadata(false, HandleShowOnTapPropertyChanged));

        public static bool GetShowOnTap(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowOnTapProperty);
        }

        public static void SetShowOnTap(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowOnTapProperty, value);
        }

        private static void HandleShowOnTapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            element.Tapped -= HandleElementTapped;

            var showOnTap = e.NewValue != null ? (bool)e.NewValue : false;
            if (showOnTap)
            {
                element.Tapped += HandleElementTapped;
            }
        }

        private static void HandleElementTapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}