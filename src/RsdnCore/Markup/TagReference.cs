namespace Rsdn.Markup
{
    using Rsdn.Markup.Rendering;

    public abstract class TagReference : ContentReference, ITagReference
    {
        protected TagReference(string tagName)
        {
            if (tagName != null)
            {
                this.Name = tagName.ToUpperInvariant();
            }
            this.RequiresClosingTag = true;
        }

        public string Name { get; private set; }
        public bool RequiresClosingTag { get; set; }
        public bool ContentsAreValue { get; set; }

        protected internal virtual string ExpandValue(Tag tag, string value)
        {
            return value;
        }

        protected internal sealed override void Render(Content content, IRenderer renderer)
        {
            Render((Tag)content, renderer);
        }

        protected virtual void Render(Tag tag, IRenderer renderer)
        {
            foreach (var content in tag)
            {
                content.Render(renderer);
            }
        }
    }
}