namespace Rsdn.Markup
{
    using Rendering;

    public class BlockTagReference : TagReference
    {
        public BlockTagReference(string tagName)
            : base(tagName)
        {
        }

        public BlockType Block { get; set; }
        public bool IsSection { get; set; }

        protected sealed override void Render(Tag tag, IRenderer renderer)
        {
            if (this.IsSection)
            {
                renderer.BeginRenderSection(this.Block, tag);
            }
            else
            {
                renderer.BeginRenderBlock(this.Block, tag);
            }

            RenderOverride(tag, renderer);

            if (this.IsSection)
            {
                renderer.EndRenderSection();
            }
            else
            {
                renderer.EndRenderBlock();
            }
        }

        protected virtual void RenderOverride(Tag tag, IRenderer renderer)
        {
            base.Render(tag, renderer);
        }
    }
}