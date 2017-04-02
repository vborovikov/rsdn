namespace Rsdn.Xaml.Interactivity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Community;
    using FontAwesome.UWP;
    using Windows.UI.Xaml.Data;

    public class PostTopicToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var topic = (PostTopic)value;

            switch (topic)
            {
                case PostTopic.Comment:
                    return FontAwesomeIcon.CommentingOutline;

                case PostTopic.Website:
                    return FontAwesomeIcon.ExternalLink;

                case PostTopic.News:
                    return FontAwesomeIcon.NewspaperOutline;

                case PostTopic.Picture:
                    return FontAwesomeIcon.PictureOutline;

                case PostTopic.Video:
                    return FontAwesomeIcon.PlayCircleOutline;

                case PostTopic.Help:
                    return FontAwesomeIcon.QuestionCircle;

                default:
                    return FontAwesomeIcon.CommentOutline;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}