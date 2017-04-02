namespace Rsdn.Text
{
    using System;

    public sealed class Symbol : Token
    {
        internal Symbol(string text, int index, int length)
            : base(text, index, length)
        {
        }

        public override TokenCategory Category => TokenCategory.Symbol;

        internal static bool TestCharacter(char c)
        {
            return Char.IsSymbol(c);
        }
    }
}