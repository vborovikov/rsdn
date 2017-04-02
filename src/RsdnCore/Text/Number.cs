namespace Rsdn.Text
{
    using System;

    public sealed class Number : Token
    {
        internal Number(string text, int index, int length)
            : base(text, index, length)
        {
        }

        public override TokenCategory Category => TokenCategory.Number;

        internal static bool TestCharacter(char c)
        {
            return Char.IsDigit(c);
        }
    }
}