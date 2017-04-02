namespace Rsdn.Text.Scanners
{
    internal interface ITokenScanner
    {
        bool TestCharacter(char c);

        int FindEndIndex(string text, int startIndex);

        Token CreateToken(string text, int index, int length);
    }
}