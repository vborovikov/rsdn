namespace Rsdn.Text
{
    public sealed class Other : Token
    {
        internal Other(string text, int index, int length)
            : base(text, index, length)
        {
        }

        public override TokenCategory Category => TokenCategory.Other;

        internal static bool TestCharacter(char c)
        {
            return true;
        }
    }
}