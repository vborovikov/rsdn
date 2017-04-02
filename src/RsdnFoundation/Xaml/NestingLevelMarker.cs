namespace Rsdn.Xaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Shapes;

    [TemplatePart(Name = TemplateParts.DepthPanel, Type = typeof(Panel))]
    public sealed class NestingLevelMarker : ContentControl
    {
        private static class TemplateParts
        {
            public const string DepthPanel = "DepthPanel";
        }

        // Using a DependencyProperty as the backing store for Depth.
        public static readonly DependencyProperty DepthProperty =
            DependencyProperty.Register(nameof(Depth), typeof(int),
                typeof(NestingLevelMarker), new PropertyMetadata(0, (d, e) => ((NestingLevelMarker)d).OnDepthChanged(e)));

        private const double LevelWidth = 6d;
        private static readonly Thickness LevelMargin = new Thickness(0d, 0d, 1d, 0d);
        private static readonly Thickness DepthMargin = new Thickness(0d, 0d, LevelWidth, 0d);

        private static Color[] LevelColors =
        {
            ColorFromHex(0x5DA5DA), // 0 blue
            ColorFromHex(0xFAA43A), // 1 orange
            ColorFromHex(0xB276B2), // 2 purple
            ColorFromHex(0x60BD68), // 3 green
            ColorFromHex(0xF17CB0), // 4 pink
            ColorFromHex(0xB2912F), // 5 brown
            ColorFromHex(0xDECF3F), // 6 yellow
            ColorFromHex(0xF15854), // 7 red
            ColorFromHex(0x4D4D4D), // 8 gray
        };

        private Panel depthPanel;

        public NestingLevelMarker()
        {
            this.DefaultStyleKey = typeof(NestingLevelMarker);
        }

        public int Depth
        {
            get { return (int)GetValue(DepthProperty); }
            set { SetValue(DepthProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.depthPanel = GetTemplateChild(TemplateParts.DepthPanel) as Panel;
        }

        private static Color ColorFromHex(uint code)
        {
            return Color.FromArgb(Byte.MaxValue, (byte)(code >> 16), (byte)(code >> 8), (byte)(code >> 0));
        }

        private static Brush GetLevelBrush(int level, int levelCount)
        {
            var levelColor = GetLevelColor(level);
            var levelBrush = new SolidColorBrush(levelColor);

            if ((levelCount - level) > 1)
                levelBrush.Opacity = 0.25d;

            return levelBrush;
        }

        private static Color GetLevelColor(int level)
        {
            return LevelColors[level % LevelColors.Length];
        }

        private void OnDepthChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.depthPanel == null)
                return;

            this.depthPanel.Children.Clear();

            if (e.NewValue == null)
                return;
            var depth = (int)e.NewValue;
            if (depth <= 0)
                return;

            this.depthPanel.Margin = DepthMargin;

            for (var i = 0; i != depth; ++i)
            {
                this.depthPanel.Children.Add(new Rectangle
                {
                    Margin = LevelMargin,
                    Fill = GetLevelBrush(i, depth),
                    Width = LevelWidth,
                });
            }
        }
    }
}