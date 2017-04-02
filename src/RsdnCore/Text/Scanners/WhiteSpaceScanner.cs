namespace Rsdn.Text.Scanners
{
	class WhiteSpaceScanner : ITokenScanner
	{
		public bool TestCharacter(char c)
		{
			return Whitespace.TestCharacter(c);
		}

		public int FindEndIndex(string text, int startIndex)
		{
			for (var i = startIndex; i != text.Length; ++i)
			{
				if (Whitespace.TestCharacter(text[i]) == false)
				{
					return i;
				}
			}

			return text.Length;
		}

		public Token CreateToken(string text, int index, int length)
		{
			return new Whitespace(text, index, length);
		}
	}
}
