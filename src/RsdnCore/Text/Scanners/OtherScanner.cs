namespace Rsdn.Text.Scanners
{
	class OtherScanner : ITokenScanner
	{
		public bool TestCharacter(char c)
		{
			return Other.TestCharacter(c);
		}

		public int FindEndIndex(string text, int startIndex)
		{
			return startIndex + 1;
		}

		public Token CreateToken(string text, int index, int length)
		{
			return new Other(text, index, length);
		}
	}
}
