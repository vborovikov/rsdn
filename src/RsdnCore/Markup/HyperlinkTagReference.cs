namespace Rsdn.Markup
{
    using System;
    using System.Linq;
    using Rsdn.Markup.Rendering;

    internal class HyperlinkTagReference : TagReference
    {
        public HyperlinkTagReference()
            : this(RsdnMarkupReference.KnownTags.Hyperlink)
        {
        }

        protected HyperlinkTagReference(string tagName)
            : base(tagName)
        {
        }

        protected override void Render(Tag tag, IRenderer renderer)
        {
            var inlineType = GetInlineType(tag.Value);

            if (inlineType == InlineType.Command)
            {
                renderer.BeginRenderInline(inlineType, tag);
                renderer.EndRenderInline();
            }
            else if (tag.OfType<Tag>().Any())
            {
                // [url] contains other tags inside of it

                renderer.BeginRenderInline(inlineType, tag);
                if (tag.Value != null)
                {
                    renderer.RenderContent(tag.Value);
                }
                renderer.EndRenderInline();

                base.Render(tag, renderer);
            }
            else
            {
                renderer.BeginRenderInline(inlineType, tag);
                if (tag.Any())
                {
                    base.Render(tag, renderer);
                }
                else
                {
                    // Sometimes people post link tags without content
                    renderer.RenderContent(tag.Value);
                }
                renderer.EndRenderInline();
            }
        }

        protected virtual InlineType GetInlineType(string uriString)
        {
            if (String.IsNullOrWhiteSpace(uriString) == false)
            {
                if (uriString.StartsWith("rsdn.ru", StringComparison.OrdinalIgnoreCase) ||
                    uriString.StartsWith("www.rsdn.ru", StringComparison.OrdinalIgnoreCase) ||
                    UriHasRsdnHost(uriString))
                {
                    return InlineType.Command;
                }
            }

            return InlineType.Hyperlink;
        }

        private static bool UriHasRsdnHost(string uriString)
        {
            Uri uri = null;
            if (Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out uri))
            {
                return uri.IsAbsoluteUri && uri.Host.Contains("rsdn.");
            }

            return false;
        }
    }
}