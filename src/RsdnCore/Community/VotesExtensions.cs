namespace Rsdn.Community
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Text;

    public static class VotesExtensions
    {
        private const int MaxEmojiCount = 5;
        private const string SuperscriptDigits = "\u2070\u00b9\u00b2\u00b3\u2074\u2075\u2076\u2077\u2078\u2079";

        public static string ToEmojiVotes(this IVotes ratings, bool expanded = false)
        {
            var emoji = new StringBuilder();

            var maxCount = expanded ? -1 : MaxEmojiCount;

            emoji
                .AppendEmojis(Votes.Interesting, ratings.InterestingCount, maxCount)
                .AppendEmojis(Votes.Thanks, ratings.ThanksCount, maxCount)
                .AppendEmojis(Votes.Excellent, ratings.ExcellentCount, maxCount)
                .AppendEmojis(Votes.Agreed, ratings.AgreedCount, maxCount)
                .AppendEmojis(Votes.Disagreed, ratings.DisagreedCount, maxCount)
                .AppendEmojis(Votes.Plus1, ratings.Plus1Count, maxCount)
                .AppendEmojis(Votes.Funny, ratings.FunnyCount, maxCount);

            return emoji.ToString();
        }

        private static StringBuilder AppendEmojis(this StringBuilder emoji, string emojiChar, int? count, int maxCount)
        {
            if (count > 0)
            {
                if (emoji.Length > 0)
                {
                    emoji.Append(Whitespace.ThinSpace);
                }

                if (count < maxCount || maxCount < 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        emoji.Append(emojiChar);
                    }
                }
                else
                {
                    emoji
                        .Append(emojiChar)
                        .Append(CountToString(count));
                }
            }

            return emoji;
        }

        private static string CountToString(int? count)
        {
            if (count == null)
                return String.Empty;

            return String.Concat(count.Value.ToString(CultureInfo.InvariantCulture).Select(ch => SuperscriptDigits[ch - '0']));
        }
    }
}