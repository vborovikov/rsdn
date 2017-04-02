namespace Rsdn.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Primitives;
    using Rendering;
    using Text;

    public class MarkupReference
    {
        internal static readonly TagReference MarkupRootReference = new RootTagReference();
        private static readonly TagReference DefaultHyperlinkTagReference = new HyperlinkTagReference();
        private static readonly EmojiReference defaultEmojiReference = new EmojiReference();
        private static readonly ContentReference defaultContentReference = new ContentReference();
        private static readonly SpecialContentReference codeKeywordContentReference = new SpecialContentReference(ContentType.CodeKeyword);

        private readonly List<TagReference> tagReferences;

        public MarkupReference()
        {
            this.tagReferences = new List<TagReference>
            {
                DefaultHyperlinkTagReference,
            };
        }

        public MarkupRoot Parse(string plainText)
        {
            var parsedText = PlainText.Parse(plainText);

            var tagStack = new Stack<Tag>();
            tagStack.Push(new MarkupRoot());

            foreach (var part in parsedText)
            {
                var currentTag = tagStack.Peek();

                if (part.IsPlainText)
                {
                    ParseContent(currentTag, part.ToString());
                }
                else
                {
                    var openingTag = part as OpeningTag;
                    if (openingTag != null)
                    {
                        currentTag = ParseOpeningTag(openingTag, currentTag, tagStack);
                    }
                    else
                    {
                        var closingTag = part as ClosingTag;
                        currentTag = ParseClosingTag(closingTag, currentTag, tagStack);
                    }
                }
            }

            // Auto-close unclosed tags.
            while (tagStack.Count > 1)
            {
                tagStack.Pop();
            }

            return tagStack.Pop() as MarkupRoot;
        }

        protected void Add(TagReference tagReference)
        {
            this.tagReferences.Add(tagReference);
        }

        protected virtual Content CreateContent(string text)
        {
            return new Content(defaultContentReference, text);
        }

        protected virtual Tag CreateHyperlink(string uriString, string content = null)
        {
            return new Tag(DefaultHyperlinkTagReference, uriString)
            {
                CreateContent(content ?? uriString)
            };
        }

        private static string FixUriString(string uriString)
        {
            if (uriString.StartsWith("mailto", StringComparison.Ordinal) == false && uriString.Contains("://") == false)
            {
                uriString = "http://" + uriString;
            }

            return uriString.TrimEnd('.', ',', ':', '!', '?');
        }

        private Tag ParseClosingTag(ClosingTag closingTag, Tag currentTag, Stack<Tag> tagStack)
        {
            if (String.Equals(currentTag.Name, closingTag.Name, StringComparison.OrdinalIgnoreCase))
            {
                tagStack.Pop();
            }
            else if (currentTag.RequiresClosingTag == false)
            {
                tagStack.Pop();
                currentTag = tagStack.Peek();

                if (String.Equals(currentTag.Name, closingTag.Name, StringComparison.OrdinalIgnoreCase))
                {
                    tagStack.Pop();
                }
                else
                {
                    currentTag.Add(CreateContent(closingTag.ToString()));
                }
            }
            else
            {
                currentTag.Add(CreateContent(closingTag.ToString()));
            }
            return currentTag;
        }

        private Tag ParseOpeningTag(OpeningTag openingTag, Tag currentTag, Stack<Tag> tagStack)
        {
            var tagReference = FindTagReference(openingTag.Name);
            if (tagReference == null)
            {
                currentTag.Add(CreateContent(openingTag.ToString()));
            }
            else
            {
                if (currentTag.RequiresClosingTag == false)
                {
                    tagStack.Pop();
                    currentTag = tagStack.Peek();
                }

                var newTag = new Tag(tagReference, openingTag.Value);
                currentTag.Add(newTag);

                if (openingTag.IsSelfClosing == false)
                {
                    tagStack.Push(newTag);
                }
            }
            return currentTag;
        }

        private void ParseContent(Tag tag, string text)
        {
            if (tag.TreatsContentsAsValue)
            {
                tag.Add(CreateContent(text));
                return;
            }

            var tokens = text.Tokenize();

            if (tag.IsCode)
            {
                ParseCode(tag, tokens);
            }
            else
            {
                ParseSpecialContent(tag, tokens);
            }
        }

        private void ParseSpecialContent(Tag tag, Token[] tokens)
        {
            var i = 0;
            while (i < tokens.Length)
            {
                if (tokens[i].Category != TokenCategory.Whitespace)
                {
                    if (ParseUri(tag, tokens, i, out i) ||
                        ParseEmoji(tag, tokens, i, out i))
                    {
                        continue;
                    }
                }

                tag.Add(CreateContent(tokens[i]));
                ++i;
            }
        }

        private bool ParseUri(Tag tag, Token[] tokens, int startIndex, out int nextIndex)
        {
            nextIndex = startIndex;

            if ((tokens.Length - startIndex) < 3)
                // less than 3 tokens left
                return false;
            if (tokens[startIndex].Category != TokenCategory.Word)
                // an uri must start from a word
                return false;

            var usualSuspects =
                tokens[startIndex].Equals("http") ||
                tokens[startIndex].Equals("https") ||
                tokens[startIndex].Equals("www");

            if (usualSuspects == false)
            {
                // it looks like ####.###
                usualSuspects =
                    tokens[startIndex + 1].Equals('.') &&
                    tokens[startIndex + 2].Category == TokenCategory.Word &&
                    tokens[startIndex + 2].Length >= 2 &&
                    tokens[startIndex + 2].Length <= 3;
            }

            if (usualSuspects == false)
            {
                return false;
            }

            var i = tokens.FindStopIndex(startIndex, '>');
            var uriString = tokens.Substring(startIndex, i - startIndex);
            if (Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
            {
                tag.Add(CreateHyperlink(FixUriString(uriString), uriString));

                nextIndex = i;
                return true;
            }

            return false;
        }

        private bool ParseEmoji(Tag tag, Token[] tokens, int startIndex, out int nextIndex)
        {
            nextIndex = startIndex;
            if ((tokens.Length - startIndex) < 3)
                // no emojis less than 3 symbols
                return false;

            var usualSuspects =
                (tokens[startIndex].Category == TokenCategory.PunctuationMark ||
                 tokens[startIndex].Category == TokenCategory.Symbol) &&
                (tokens[startIndex].Equals(':') || tokens[startIndex].Equals(';'));

            if (usualSuspects == false)
                return false;

            var emojiTkLength = 0;

            if (tokens[startIndex + 1].Category == TokenCategory.Word &&
                tokens[startIndex + 2].Equals(':'))
            {
                // :###:
                emojiTkLength = 3;
            }
            else
            {
                // :???: :) ;)
                var i = tokens.FindStopIndex(startIndex + 1, ':', ';');
                emojiTkLength = i - startIndex;
                if (i < tokens.Length && tokens[i].Equals(':'))
                    ++emojiTkLength;
            }

            var emojiStr = tokens.Substring(startIndex, emojiTkLength);
            SpecialContent emoji;
            var foundEmoji = defaultEmojiReference.TryCreate(emojiStr, out emoji);
            if (foundEmoji == false && emojiStr[emojiStr.Length - 1] == ':')
            {
                // last ':' might be part of another emoji
                --emojiTkLength;
                emojiStr = emojiStr.TrimEnd(':');
                foundEmoji = defaultEmojiReference.TryCreate(emojiStr, out emoji);
            }

            if (foundEmoji)
            {
                tag.Add(emoji);

                nextIndex = startIndex + emojiTkLength;
                return true;
            }

            return false;
        }

        private void ParseCode(Tag tag, Token[] tokens)
        {
            var codeReference = tag.Reference as ICodeBlockTagReference;

            for (var i = 0; i < tokens.Length; ++i)
            {
                Content tkContent = null;

                if (tokens[i].Category == TokenCategory.Word)
                {
                    // check if the word is a keyword
                    var tkKeyword = tokens[i];
                    if (codeReference?.IsKeyword(tkKeyword) == true)
                    {
                        tkContent = new SpecialContent(codeKeywordContentReference, tkKeyword);
                    }
                }
                //todo: parse comments (check for special symbols)

                if (tkContent == null)
                    tkContent = CreateContent(tokens[i]);

                tag.Add(tkContent);
            }
        }

        private TagReference FindTagReference(string tagName)
        {
            tagName = tagName.ToUpperInvariant();

            var tagReference =
                this.tagReferences
                .FirstOrDefault(tag =>
                    String.Equals(tag.Name, tagName, StringComparison.OrdinalIgnoreCase));

            return tagReference;
        }
    }
}