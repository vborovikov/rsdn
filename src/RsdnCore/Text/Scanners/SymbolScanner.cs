namespace Rsdn.Text.Scanners
{
	class SymbolScanner : ITokenScanner
	{
		public bool TestCharacter(char c)
		{
			return Symbol.TestCharacter(c);
		}

		public int FindEndIndex(string text, int startIndex)
		{
			return startIndex + 1;
		}

		public Token CreateToken(string text, int index, int length)
		{
			return new Symbol(text, index, length);
		}
	}
}
