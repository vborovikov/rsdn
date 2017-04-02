namespace Rsdn.Text.Scanners
{
	class NumberScanner : ITokenScanner
	{
		public bool TestCharacter(char c)
		{
			return Number.TestCharacter(c);
		}

		public int FindEndIndex(string text, int startIndex)
		{
			for (var i = startIndex; i != text.Length; ++i)
			{
				if (Number.TestCharacter(text[i]) == false)
				{
					return i;
				}
			}

			return text.Length;
		}

		public Token CreateToken(string text, int index, int length)
		{
			return new Number(text, index, length);
		}
	}
}
