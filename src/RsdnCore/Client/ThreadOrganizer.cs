#undef PRINT_LEVELS

namespace Rsdn.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Community;
    using Community.Interaction;
    using Markup;

    public static class ThreadOrganizer
    {
        public const int RootLevel = 0;
        public const int TopLevel = 1;

        public static readonly ForumSymbols UnknownForumSymbols = new ForumSymbols(-1, "Uk", "Ukn");

        private static readonly string[] videoSites =
                {
            "youtube.com",
            "youtu.be",
            "coub.com",
            "vimeo.com",
        };

        private static readonly string[] newsSites =
        {
            "meduza.io",
            "nplus1.ru",
            "bbc.com",
            "cnn.com",
            "newsru.com",
        };

        private static readonly IDictionary<int, ForumSymbols> forumSymbolsMap = new Dictionary<int, ForumSymbols>
        {
            { 1, new ForumSymbols(1, "Ос", "Оса") },
            { 3, new ForumSymbols(3, "Wi", "WIN") },
            { 4, new ForumSymbols(4, "Co", "COM") },
            { 5, new ForumSymbols(5, "Mf", "MFC") },
            { 6, new ForumSymbols(6, "DB", "DB") },
            { 7, new ForumSymbols(7, "Wt", "WTL") },
            { 8, new ForumSymbols(8, ".N", ".Nt") },
            { 9, new ForumSymbols(9, "CC", "C++") },
            { 10, new ForumSymbols(10, "Jv", "Jva") },
            { 11, new ForumSymbols(11, "Nt", "Net") },
            { 12, new ForumSymbols(12, "Wb", "Web") },
            { 13, new ForumSymbols(13, "Ра", "Раб") },
            { 14, new ForumSymbols(14, "Xm", "Xml") },
            { 15, new ForumSymbols(15, "Al", "Alg") },
            { 16, new ForumSymbols(16, "Ps", "Pas") },
            { 17, new ForumSymbols(17, "Ar", "Arh") },
            { 18, new ForumSymbols(18, "Пр", "Прч") },
            { 19, new ForumSymbols(19, "Md", "Mda") },
            { 20, new ForumSymbols(20, "Жр", "Жур") },
            { 21, new ForumSymbols(21, "VB", "VB") },
            { 22, new ForumSymbols(22, "Un", "Unx") },
            { 24, new ForumSymbols(24, "In", "Ins") },
            { 25, new ForumSymbols(25, ".W", ".Nw") },
            { 26, new ForumSymbols(26, "Am", "Asm") },
            { 27, new ForumSymbols(27, "Ph", "Phs") },
            { 28, new ForumSymbols(28, "Id", "Ide") },
            { 29, new ForumSymbols(29, "Sc", "Src") },
            { 30, new ForumSymbols(30, "@H", "R@H") },
            { 31, new ForumSymbols(31, "Нв", "Нов") },
            { 32, new ForumSymbols(32, "Nt", "Ntp") },
            { 33, new ForumSymbols(33, "КУ", "КУл") },
            { 34, new ForumSymbols(34, "Жз", "ЖЗЛ") },
            { 35, new ForumSymbols(35, "Fq", "Faq") },
            { 37, new ForumSymbols(37, "Эт", "Этю") },
            { 40, new ForumSymbols(40, "Бз", "Биз") },
            { 41, new ForumSymbols(41, "If", "Inf") },
            { 42, new ForumSymbols(42, "Gm", "Gam") },
            { 43, new ForumSymbols(43, "T1", "Ts1") },
            { 45, new ForumSymbols(45, "PD", "PDA") },
            { 48, new ForumSymbols(48, "СВ", "СВ") },
            { 52, new ForumSymbols(52, "Op", "Opn") },
            { 53, new ForumSymbols(53, ".G", ".Ng") },
            { 56, new ForumSymbols(56, "BL", "BLT") },
            { 59, new ForumSymbols(59, "Рп", "Рпр") },
            { 61, new ForumSymbols(61, "Пе", "Прв") },
            { 62, new ForumSymbols(62, "T2", "Ts2") },
            { 63, new ForumSymbols(63, "HW", "HW") },
            { 65, new ForumSymbols(65, "##", "###") },
            { 68, new ForumSymbols(68, "##", "###") },
            { 69, new ForumSymbols(69, "##", "###") },
            { 71, new ForumSymbols(71, "##", "###") },
            { 72, new ForumSymbols(72, "Ts", "Tst") },
            { 73, new ForumSymbols(73, "Ca", "CCa") },
            { 74, new ForumSymbols(74, "</", "</>") },
            { 76, new ForumSymbols(76, "Уп", "Упр") },
            { 78, new ForumSymbols(78, "UI", "UI") },
            { 83, new ForumSymbols(83, "КВ", "КСВ") },
            { 84, new ForumSymbols(84, "Пл", "Плт") },
            { 85, new ForumSymbols(85, "На", "Нау") },
            { 87, new ForumSymbols(87, "Nv", "Nvs") },
            { 90, new ForumSymbols(90, "jB", "jB") },
            { 91, new ForumSymbols(91, "Рц", "Рцп") },
            { 92, new ForumSymbols(92, "Рк", "Рак") },
            { 93, new ForumSymbols(93, "Ир", "Ира") },
            { 94, new ForumSymbols(94, "TI", "Tif") },
            { 95, new ForumSymbols(95, "Nm", "Nmr") },
            { 99, new ForumSymbols(99, "Dy", "Dyn") },
            { 100, new ForumSymbols(100, "Qt", "Qt") },
            { 101, new ForumSymbols(101, "За", "Згр") },
            { 103, new ForumSymbols(103, "Sc", "Sec") },
            { 104, new ForumSymbols(104, "Пу", "Пуб") },
            { 105, new ForumSymbols(105, "Ap", "Apl") },
            { 107, new ForumSymbols(107, "Бл", "Блг") },
            { 108, new ForumSymbols(108, "Ав", "АМВ") },
            { 111, new ForumSymbols(111, "Сд", "Сдс") },
        };

        private static readonly Regex replyTitleTemplate = new Regex(@"^Re(?:\[(?<level>\d+)\])?:\s(?<subj>.*)");

        public static IList<PostModel> Organize(IEnumerable<PostModel> posts, ThreadModel thread)
        {
            var threadPosts = new LinkedList<PostModel>();

            // We assume that posts are already sorted by the posting time
            foreach (var post in posts)
            {
                SetPostFreshness(thread, post);

                // Determine the level of the post and find the subthread opening post.
                // Then, update positions of posts.
                if (post.SubthreadId == post.ThreadId)
                {
                    post.Level = TopLevel;
                }
                else
                {
                    LinkedListNode<PostModel> subthreadNode = null;

                    // We search for the subthread start from the last reply because most likely it is there.
                    for (var node = threadPosts.Last; node != null; node = node.Previous)
                    {
                        if (node.Value.Id == post.SubthreadId)
                        {
                            subthreadNode = node;
                            break;
                        }
                    }
                    if (subthreadNode != null)
                    {
                        post.Level = subthreadNode.Value.Level + 1;

                        // Find the next node with the same level as the subthread start level and place the post before it.

                        LinkedListNode<PostModel> nextSubthreadNode = null;
                        for (var node = subthreadNode.Next; node != null; node = node.Next)
                        {
                            if (node.Value.Level <= subthreadNode.Value.Level)
                            {
                                nextSubthreadNode = node;
                                break;
                            }
                        }

                        if (nextSubthreadNode != null)
                        {
                            var postNode = threadPosts.AddBefore(nextSubthreadNode, post);

                            for (var node = postNode; node != null; node = node.Next)
                            {
                                if (node.Next != null)
                                {
                                    node.Value.Position = node.Next.Value.Position;
                                }
                                else
                                {
                                    node.Value.Position = threadPosts.Count;
                                }
                            }

#if PRINT_LEVELS
                            Debug.WriteLine("Ins pos {0} lvl {1}: {2} [{3}] ({4})", post.Position, post.Level,
                                post.Title, post.Username, post.Posted.ToString("g"));
#endif
                            continue;
                        }
                    }
                }

                // This is a top level reply post or the list is corrupted
                threadPosts.AddLast(post);
                if (post.ThreadId == null)
                {
                    post.Position = 0; // zero position is for the thread opening post
                    post.Level = RootLevel;
                }
                else
                {
                    post.Position = threadPosts.Count;
                }

#if PRINT_LEVELS
                Debug.WriteLine("Add pos {0} lvl {1}: {2} [{3}] ({4})", post.Position, post.Level,
                    post.Title, post.Username, post.Posted.ToString("g"));
#endif
            }

            return threadPosts.ToArray();
        }

        public static string GetReplyMessage(PostModel post)
        {
            var reply = new StringBuilder();

            reply
                .AppendFormat("Здравствуйте, {0}, вы писали:", post.Username)
                .AppendLine()
                .AppendLine();

            using (var messageReader = new StringReader(post.Message))
            {
                var usernameAbbr = AbbreviateUsername(post.Username);

                string para;
                while ((para = messageReader.ReadLine()) != null)
                {
                    if (para.Length > 0)
                    {
                        var citeIndex = CiteContentReference.FindCiteIndex(para);
                        if (citeIndex > -1)
                        {
                            para = para.Insert(citeIndex, CiteContentReference.CiteChar.ToString());
                        }
                        else
                        {
                            reply
                                .Append(usernameAbbr)
                                .Append(CiteContentReference.CiteChar);
                        }
                    }

                    reply.AppendLine(para);
                }
            }

            return reply.ToString();
        }

        public static IEnumerable<ThreadModel> Organize(IEnumerable<ThreadModel> threads, ForumModel forum)
        {
            foreach (var thread in threads)
            {
                SetThreadFreshness(forum, thread);
            }

            return threads;
        }

        public static PostTopic DeterminePostTopic(MarkupRoot message, string title)
        {
            var imageTag = message.FindTag(tag => tag.Name == RsdnMarkupReference.KnownTags.Image);
            if (imageTag != null)
                return PostTopic.Picture;

            var linkTag = message.FindTag(tag => tag.Name == RsdnMarkupReference.KnownTags.Hyperlink);
            if (linkTag != null)
            {
                Uri uri = null;
                if (Uri.TryCreate(linkTag.Value, UriKind.RelativeOrAbsolute, out uri) == false)
                    return PostTopic.Website;

                var uriHost = uri.IsAbsoluteUri ? uri.Host : String.Empty;
                if (String.IsNullOrWhiteSpace(uriHost) == false)
                {
                    if (videoSites.Any(videoSite => uriHost.EndsWith(videoSite, StringComparison.OrdinalIgnoreCase)))
                        return PostTopic.Video;

                    if (newsSites.Any(newsSite => uriHost.EndsWith(newsSite, StringComparison.OrdinalIgnoreCase)) ||
                        linkTag.Value.ToUpperInvariant().Contains("NEWS"))
                        return PostTopic.News;
                }

                if (uriHost.StartsWith("rsdn.", StringComparison.OrdinalIgnoreCase) == false)
                    return PostTopic.Website;
            }

            var quoteTag = message.FindTag(tag => tag.Name == RsdnMarkupReference.KnownTags.Quote);
            if (quoteTag != null)
            {
                return PostTopic.Comment;
            }

            var text = message.ToString();

            if (String.IsNullOrWhiteSpace(text))
                return PostTopic.Unknown;

            var helpSignCount = text.Count(ch => ch == '?');
            if (helpSignCount >= 1 || title.EndsWith("?", StringComparison.Ordinal))
                return PostTopic.Help;

            return PostTopic.Unknown;
        }

        public static string GetReplyTitle(string postTitle)
        {
            var replyTitleMatch = replyTitleTemplate.Match(postTitle);

            if (replyTitleMatch.Success)
            {
                var level = 2;
                var levelGroup = replyTitleMatch.Groups["level"];
                if (levelGroup.Success && Int32.TryParse(levelGroup.Value, out level))
                {
                    ++level;
                }

                return String.Format("Re[{0}]: {1}", level, replyTitleMatch.Groups["subj"].Value);
            }

            return "Re: " + postTitle;
        }

        public static ForumSymbols GetForumSymbols(int forumId)
        {
            ForumSymbols symbols;
            if (forumSymbolsMap.TryGetValue(forumId, out symbols))
                return symbols;

            return UnknownForumSymbols;
        }

        private static string AbbreviateUsername(string username)
        {
            if (username.Length <= 3)
                return String.Concat(username.Where(ch => Char.IsSeparator(ch) == false));

            if (username.Any(Char.IsSeparator))
            {
                var caps = String.Concat(username
                    .Where((ch, i) => Char.IsSeparator(ch) == false && (i == 0 || Char.IsSeparator(username[i - 1])))
                    .Take(3)).ToUpper();

                return caps;
            }

            var shortName = String.Concat(username.Where(Char.IsUpper).Take(3));
            return shortName.Length > 0 ? shortName : username.Substring(0, 1).ToUpper();
        }

        private static void SetThreadFreshness(ForumModel forum, ThreadModel thread)
        {
            thread.IsNew = thread.Updated > (forum.Visited ?? DateTime.MinValue);
        }

        private static void SetPostFreshness(ThreadModel thread, PostModel post)
        {
            post.IsNew = (post.Updated ?? post.Posted) > (thread.Viewed ?? DateTime.MinValue);
        }
    }
}