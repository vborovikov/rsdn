namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Relay.PresentationModel;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Input;

    public static class ElementCommands
    {
        public static readonly DependencyProperty DoubleTapProperty =
            DependencyProperty.RegisterAttached("DoubleTap", typeof(ICommand), typeof(ElementCommands),
                new PropertyMetadata(null, HandleDoubleTapPropertyChanged));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ElementCommands), new PropertyMetadata(null));

        public static ICommand GetDoubleTap(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DoubleTapProperty);
        }

        public static void SetDoubleTap(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleTapProperty, value);
        }

        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        private static void HandleDoubleTapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element == null)
                return;

            element.DoubleTapped -= HandleElementDoubleTapped;
            if (e.NewValue != null)
                element.DoubleTapped += HandleElementDoubleTapped;
        }

        private static void HandleElementDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var element = (UIElement)sender;
            GetDoubleTap(element)?.TryExecute(GetCommandParameter(element));
        }
    }
}