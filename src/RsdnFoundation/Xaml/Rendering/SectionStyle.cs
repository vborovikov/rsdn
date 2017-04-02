namespace Rsdn.Xaml.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Documents;
    using Windows.UI.Xaml.Media;

    internal struct SectionStyle
    {
        public Brush Foreground;
        public FontFamily FontFamily;
        public double? FontSize;
        public Thickness? Margin;

        public static SectionStyle Compute(SectionStyle left, SectionStyle right)
        {
            var sectionStyle = new SectionStyle
            {
                Foreground = right.Foreground ?? left.Foreground,
                FontFamily = right.FontFamily ?? left.FontFamily,
                FontSize = right.FontSize ?? left.FontSize,
                Margin = right.Margin ?? left.Margin,
            };

            if (left.Margin != null && right.Margin != null)
            {
                sectionStyle.Margin = new Thickness(
                    left.Margin.Value.Left + right.Margin.Value.Left,
                    left.Margin.Value.Top + right.Margin.Value.Top,
                    left.Margin.Value.Right + right.Margin.Value.Right,
                    left.Margin.Value.Bottom + right.Margin.Value.Bottom
                );
            }

            return sectionStyle;
        }

        public void Apply(Block block)
        {
            if (this.Foreground != null)
            {
                block.Foreground = this.Foreground;
            }
            if (this.FontFamily != null)
            {
                block.FontFamily = this.FontFamily;
            }
            if (this.FontSize != null)
            {
                block.FontSize = this.FontSize.Value;
            }
            if (this.Margin != null)
            {
                block.Margin = this.Margin.Value;
            }
        }
    }
}