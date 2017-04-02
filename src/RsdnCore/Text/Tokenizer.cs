namespace Rsdn.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Scanners;

    public static class Tokenizer
    {
        private static readonly ITokenScanner[] tokenScanners =
        {
            new WhiteSpaceScanner(),
            new WordScanner(),
            new NumberScanner(),
            new PunctuationMarkScanner(),
            new SymbolScanner(),
            new OtherScanner()
        };

        public static Token[] Tokenize(this string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var tokens = new List<Token>();

            var nextPosition = 0;
            for (var tokenStartIndex = 0; tokenStartIndex < text.Length; tokenStartIndex = nextPosition)
            {
                var tokenScanner = tokenScanners.First(scanner => scanner.TestCharacter(text[tokenStartIndex]));

                var tokenEndIndex = tokenScanner.FindEndIndex(text, tokenStartIndex);
                if (tokenEndIndex < 0)
                    tokenEndIndex = text.Length;

                tokens.Add(tokenScanner.CreateToken(text, tokenStartIndex, tokenEndIndex - tokenStartIndex));
                nextPosition = tokenEndIndex;
            }

            return tokens.ToArray();
        }

        public static string Substring(this Token[] tokens, int index, int count)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));
            if (tokens.Length == 0)
                throw new ArgumentException(nameof(tokens));
            if (count < 0 || count > tokens.Length)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index < 0 || index >= tokens.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            if ((index + count) > tokens.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            var srcIndex = tokens[index].Index;
            var srcLength = 0;
            var afterLastIndex = index + count;
            if (afterLastIndex == tokens.Length)
            {
                var lastIndex = tokens.Length - 1;
                srcLength = tokens[lastIndex].Index + tokens[lastIndex].Length - srcIndex;
            }
            else
            {
                srcLength = tokens[afterLastIndex].Index - srcIndex;
            }

            return tokens[0].Source.Substring(srcIndex, srcLength);
        }

        public static int FindStopIndex(this Token[] tokens, int startIndex, params char[] stopChars)
        {
            var i = startIndex;
            while (tokens[i].Category != TokenCategory.Whitespace &&
                   stopChars.Any(tokens[i].Equals) == false)
            {
                if (++i == tokens.Length)
                    break;
            }

            return i;
        }
    }
}