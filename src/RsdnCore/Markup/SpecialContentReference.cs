namespace Rsdn.Markup
{
    using Rendering;

    public class SpecialContentReference : ContentReference
    {
        public SpecialContentReference(ContentType contentType)
        {
            this.ContentType = contentType;
        }

        public sealed override ContentType ContentType { get; }

        protected override void RenderLine(string line, IRenderer renderer)
        {
            renderer.RenderContent(line, this.ContentType);
        }
    }
}