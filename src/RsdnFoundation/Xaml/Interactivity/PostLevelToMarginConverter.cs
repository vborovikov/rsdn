namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class PostLevelToMarginConverter : IValueConverter
    {
        private const int MaxLevel = 10;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var level = (int)value;

            return new Thickness(Math.Min(MaxLevel, level) * 12d, 0d, 0d, 0d);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}