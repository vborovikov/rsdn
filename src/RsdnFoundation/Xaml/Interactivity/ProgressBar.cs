namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community.Presentation;
    using Windows.UI.Xaml;
    using XamlProgressBar = Windows.UI.Xaml.Controls.ProgressBar;

    public static class ProgressBar
    {
        // Using a DependencyProperty as the backing store for ItemsState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsStateProperty =
            DependencyProperty.RegisterAttached("ItemsState",
                typeof(ItemsState), typeof(ProgressBar),
                new PropertyMetadata(ItemsState.Loaded, HandleItemsStatePropertyChanged));

        // Using a DependencyProperty as the backing store for BusyState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BusyStateProperty =
            DependencyProperty.RegisterAttached("BusyState",
                typeof(bool), typeof(ProgressBar),
                new PropertyMetadata(false, HandleBusyStatePropertyChanged));

        public static ItemsState GetItemsState(DependencyObject obj)
        {
            return (ItemsState)obj.GetValue(ItemsStateProperty);
        }

        public static void SetItemsState(DependencyObject obj, ItemsState value)
        {
            obj.SetValue(ItemsStateProperty, value);
        }

        public static bool GetBusyState(DependencyObject obj)
        {
            return (bool)obj.GetValue(BusyStateProperty);
        }

        public static void SetBusyState(DependencyObject obj, bool value)
        {
            obj.SetValue(BusyStateProperty, value);
        }

        private static void HandleBusyStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var busyState = e.NewValue != null ? (bool)e.NewValue : false;
            ToggleProgressBar(d, busyState);
        }

        private static void HandleItemsStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newState = e.NewValue != null ? (ItemsState)e.NewValue : ItemsState.Empty;
            ToggleProgressBar(d, show: newState == ItemsState.Loading);
        }

        private static void ToggleProgressBar(DependencyObject d, bool show)
        {
            var progressBar = d as XamlProgressBar;
            if (progressBar != null)
            {
                progressBar.IsIndeterminate = show;
                progressBar.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}