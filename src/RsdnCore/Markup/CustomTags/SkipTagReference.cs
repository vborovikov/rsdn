namespace Rsdn.Markup.CustomTags
{
    using Rendering;

    public class SkipTagReference : InlineTagReference
    {
        public SkipTagReference()
            : base(RsdnMarkupReference.CustomTags.Skip)
        {
            this.RequiresClosingTag = false;
            this.Inline = InlineType.Span;
        }

        protected override void RenderOverride(Tag tag, IRenderer renderer)
        {
            renderer.RenderContent("\u2702"); // ✂
        }
    }
}