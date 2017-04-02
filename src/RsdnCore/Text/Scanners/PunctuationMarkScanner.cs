namespace Rsdn.Text.Scanners
{
	class PunctuationMarkScanner : ITokenScanner
	{
		public bool TestCharacter(char c)
		{
			return PunctuationMark.TestCharacter(c);
		}

		public int FindEndIndex(string text, int startIndex)
		{
			return startIndex + 1;
		}

		public Token CreateToken(string text, int index, int length)
		{
			return new PunctuationMark(text, index, length);
		}
	}
}
