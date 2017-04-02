namespace Rsdn.Markup
{
    using Rendering;

    public class InlineTagReference : TagReference
    {
        public InlineTagReference(string tagName)
            : base(tagName)
        {
        }

        public InlineType Inline { get; set; }

        protected sealed override void Render(Tag tag, IRenderer renderer)
        {
            renderer.BeginRenderInline(this.Inline, tag);
            RenderOverride(tag, renderer);
            renderer.EndRenderInline();
        }

        protected virtual void RenderOverride(Tag tag, IRenderer renderer)
        {
            base.Render(tag, renderer);
        }
    }
}