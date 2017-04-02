namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;

    public class PostTopicToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var topic = (PostTopic)value;

            switch (topic)
            {
                case PostTopic.Comment:
                    return Symbol.Comment;

                case PostTopic.Website:
                    return Symbol.Link;

                case PostTopic.News:
                    return Symbol.Globe;

                case PostTopic.Picture:
                    return Symbol.Pictures;

                case PostTopic.Video:
                    return Symbol.Video;

                case PostTopic.Help:
                    return Symbol.Help;

                default:
                    return Symbol.Message;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}