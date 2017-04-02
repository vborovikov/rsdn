namespace Rsdn.Xaml.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class GlyphMenuFlyoutItem : MenuFlyoutItem
    {
        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register(nameof(Glyph), typeof(object),
                typeof(GlyphMenuFlyoutItem), new PropertyMetadata(null));

        public GlyphMenuFlyoutItem()
        {
            this.DefaultStyleKey = typeof(GlyphMenuFlyoutItem);
        }

        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }
    }
}