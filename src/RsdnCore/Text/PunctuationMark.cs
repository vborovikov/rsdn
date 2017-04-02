namespace Rsdn.Text
{
    using System;

    public sealed class PunctuationMark : Token
    {
        internal PunctuationMark(string text, int index, int length)
            : base(text, index, length)
        {
        }

        public override TokenCategory Category => TokenCategory.PunctuationMark;

        internal static bool TestCharacter(char c)
        {
            return Char.IsPunctuation(c);
        }
    }
}