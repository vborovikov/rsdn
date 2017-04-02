namespace Rsdn.Text
{
    using System;
    using System.Globalization;

    public sealed class Word : Token
    {
        internal Word(string text, int index, int length)
            : base(text, index, length)
        {
        }

        public override TokenCategory Category => TokenCategory.Word;

        internal static bool TestCharacter(char c)
        {
            if (Char.IsLetter(c))
            {
                return true;
            }

            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            return category == UnicodeCategory.NonSpacingMark || category == UnicodeCategory.Format;
        }
    }
}