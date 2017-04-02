namespace Rsdn.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Client;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    [TemplatePart(Name = TemplateParts.LayoutRoot, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TemplateParts.LogoPresenter, Type = typeof(ContentPresenter))]
    public sealed class ForumLogo : Control
    {
        private static class TemplateParts
        {
            public const string LayoutRoot = "LayoutRoot";
            public const string LogoPresenter = "LogoPresenter";
        }

        public static readonly DependencyProperty ForumIdProperty =
            DependencyProperty.Register(nameof(ForumId), typeof(object),
                typeof(ForumLogo), new PropertyMetadata(null, HandleForumIdPropertyChanged));

        private FrameworkElement layoutRoot;
        private ContentPresenter logoPresenter;

        public ForumLogo()
        {
            this.DefaultStyleKey = typeof(ForumLogo);
        }

        public object ForumId
        {
            get { return GetValue(ForumIdProperty); }
            set { SetValue(ForumIdProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.layoutRoot = GetTemplateChild(TemplateParts.LayoutRoot) as FrameworkElement;
            this.logoPresenter = GetTemplateChild(TemplateParts.LogoPresenter) as ContentPresenter;
        }

        private static void HandleForumIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ForumLogo)?.OnForumIdChanged(e);
        }

        private void OnForumIdChanged(DependencyPropertyChangedEventArgs e)
        {
            ChangeForumSymbol();
        }

        private void ChangeForumSymbol()
        {
            var symbols = ThreadOrganizer.UnknownForumSymbols;

            var newValue = this.ForumId;
            if (newValue != null)
            {
                if (newValue is int)
                {
                    var newId = (int)newValue;
                    symbols = ThreadOrganizer.GetForumSymbols(newId);
                }
            }

            if (this.logoPresenter != null)
            {
                this.logoPresenter.Content = this.Width < 48d ? symbols.ShortSymbol : symbols.MediumSymbol;
            }
        }
    }
}