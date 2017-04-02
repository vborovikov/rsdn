namespace Rsdn.Text
{
    using System;

    public sealed class Whitespace : Token
    {
        public const char HairSpace = '\u200A';
        public const char ThinSpace = '\u2009';

        internal Whitespace(string text, int index, int length)
            : base(text, index, length)
        {
        }

        public override TokenCategory Category => TokenCategory.Whitespace;

        internal static bool TestCharacter(char c)
        {
            return Char.IsWhiteSpace(c);
        }
    }
}