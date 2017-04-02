namespace Rsdn.Markup
{
	using Rsdn.Markup.Rendering;

	sealed class EmailTagReference : HyperlinkTagReference
	{
		public EmailTagReference()
			: base(RsdnMarkupReference.KnownTags.Email)
		{
			this.ContentsAreValue = true;
		}

		protected internal override string ExpandValue(Tag tag, string value)
		{
			return @"mailto:" + value;
		}

		protected override InlineType GetInlineType(string uriString)
		{
			return InlineType.Hyperlink;
		}
	}
}
