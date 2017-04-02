namespace Rsdn.Markup
{
	using System;
	using System.Globalization;
	using Rsdn.Markup.Rendering;

	sealed class MsdnTagReference : HyperlinkTagReference
	{
		public MsdnTagReference()
			: base(RsdnMarkupReference.KnownTags.Msdn)
		{
			this.ContentsAreValue = true;
		}

		protected internal override string ExpandValue(Tag tag, string value)
		{
			return String.Format(@"http://social.msdn.microsoft.com/Search/{0}/?Query={1}",
				CultureInfo.CurrentCulture.Name, Uri.EscapeDataString(value));
		}


		protected override InlineType GetInlineType(string uriString)
		{
			return InlineType.Hyperlink;
		}
	}
}
