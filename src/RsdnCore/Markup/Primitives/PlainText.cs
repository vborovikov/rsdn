namespace Rsdn.Markup.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class PlainText : IEquatable<PlainText>
    {
        private readonly string content;

        protected PlainText(string content)
        {
            this.content = content;
        }

        public virtual bool IsPlainText { get { return true; } }

        public static IEnumerable<PlainText> Parse(string text)
        {
            var tokens = new List<PlainText>();

            // Standardize line endings
            text = text.Trim('\r', '\n', '\t', ' ').Replace("\r\n", "\n").Replace("\r", "\n");

            var openBracketPos = 0;
            var curPos = 0;
            while (curPos < text.Length)
            {
                // Find next open bracket
                openBracketPos = text.IndexOf('[', openBracketPos);

                if (openBracketPos < curPos)
                {
                    // No more tags
                    tokens.Add(new PlainText(text.Substring(curPos)));
                    break;
                }
                else if (openBracketPos == curPos)
                {
                    // A tag is right here
                    var closeBracketPos = text.IndexOf(']', openBracketPos);
                    if (closeBracketPos < 0)
                    {
                        // That tag is broken, no more tags
                        tokens.Add(new PlainText(text.Substring(curPos)));
                        break;
                    }
                    else
                    {
                        if (closeBracketPos == (openBracketPos + 1))
                        {
                            // That tag is empty, continue search for open bracket
                            tokens.Add(new PlainText(text.Substring(curPos, closeBracketPos - openBracketPos + 1)));
                            curPos = openBracketPos = closeBracketPos + 1;
                        }
                        else
                        {
                            var rawTag = text.Substring(curPos + 1, closeBracketPos - openBracketPos - 1);
                            if (rawTag.StartsWith("/", StringComparison.Ordinal))
                            {
                                rawTag = rawTag.Substring(1);
                                if (rawTag.Cast<char>().All(ch => ch != '[' && ch != ']' && ch != '/'))
                                {
                                    // Correct close tag
                                    tokens.Add(new ClosingTag(rawTag));
                                    // Continue with another portion of content
                                    curPos = openBracketPos = closeBracketPos + 1;
                                }
                                else
                                {
                                    // Incorrect close tag, continue search for open bracket
                                    tokens.Add(new PlainText(text.Substring(curPos, closeBracketPos - openBracketPos + 1)));
                                    curPos = openBracketPos = closeBracketPos + 1;
                                }
                            }
                            else
                            {
                                string tagParam = null;
                                var ep = rawTag.IndexOf('=');
                                if (ep > 0 && ep < (rawTag.Length - 1))
                                {
                                    tagParam = rawTag.Substring(ep + 1);
                                    rawTag = rawTag.Substring(0, ep);
                                }

                                var isClosed = rawTag.EndsWith("/", StringComparison.Ordinal);
                                if (isClosed && tagParam == null)
                                {
                                    // self-closing tag cannot be parameterized
                                    rawTag = rawTag.Substring(0, rawTag.Length - 1).TrimEnd();
                                }

                                if (rawTag.Cast<char>().All(ch => ch != '[' && ch != ']' && ch != '/'))
                                {
                                    // Correct open tag
                                    tokens.Add(new OpeningTag(rawTag, tagParam, isClosed));
                                    // Continue with another portion of content
                                    curPos = openBracketPos = closeBracketPos + 1;
                                }
                                else
                                {
                                    // Incorrect open tag, continue search for open bracket
                                    tokens.Add(new PlainText(text.Substring(curPos, closeBracketPos - openBracketPos + 1)));
                                    curPos = openBracketPos = closeBracketPos + 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // There is a tag after content
                    tokens.Add(new PlainText(text.Substring(curPos, openBracketPos - curPos)));
                    curPos = openBracketPos;
                }
            }

            return tokens.ToArray();
        }

        public static bool operator ==(PlainText left, PlainText right)
        {
            if (Object.ReferenceEquals(left, right))
                return true;

            if (Object.ReferenceEquals(left, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(PlainText left, PlainText right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return this.content;
        }

        public bool Equals(PlainText other)
        {
            if (Object.ReferenceEquals(other, null))
                return false;

            return Object.ReferenceEquals(this, other) ||
                String.Equals(this.content, other.content, StringComparison.CurrentCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PlainText;
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.content.GetHashCode();
        }
    }
}