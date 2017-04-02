namespace Rsdn.Markup
{
    using Rendering;

    internal sealed class ImageTagReference : HyperlinkTagReference
    {
        internal ImageTagReference()
            : base(RsdnMarkupReference.KnownTags.Image)
        {
            this.ContentsAreValue = true;
        }

        protected override void Render(Tag tag, IRenderer renderer)
        {
            renderer.BeginRenderInline(InlineType.Image, tag);
            renderer.EndRenderInline();

            //renderer.BeginRenderBlock(BlockType.Paragraph, tag);
            //base.Render(tag, renderer);
            //renderer.EndRenderBlock();
        }

        protected override InlineType GetInlineType(string uriString)
        {
            return InlineType.Hyperlink;
        }
    }
}