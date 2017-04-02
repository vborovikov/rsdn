namespace Rsdn.Text.Scanners
{
	class WordScanner : ITokenScanner
	{
		public bool TestCharacter(char c)
		{
			return Word.TestCharacter(c);
		}

		public int FindEndIndex(string text, int startIndex)
		{
			for (var i = startIndex; i < text.Length; ++i)
			{
				if (Word.TestCharacter(text[i]) == false)
				{
					var nextIndex = i + 1;
					if (text[i] == '\'' && nextIndex < text.Length && Word.TestCharacter(text[nextIndex]))
					{
						var nextNextIndex = nextIndex + 1;
						if (nextNextIndex < text.Length && Word.TestCharacter(text[nextNextIndex]) == false)
							continue;
					}
					return i;
				}
			}

			return text.Length;
		}

		public Token CreateToken(string text, int index, int length)
		{
			return new Word(text, index, length);
		}
	}
}
