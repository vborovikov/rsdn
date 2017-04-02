namespace Rsdn.Markup
{
    using System;
    using System.IO;
    using Rendering;

    public class ContentReference
    {
        public virtual ContentType ContentType { get { return ContentType.Text; } }

        protected internal virtual void Render(Content content, IRenderer renderer)
        {
            var contentStr = content.ToString();
            if (String.IsNullOrEmpty(contentStr))
                return;

            var contentEnds = contentStr.Length > 0 && contentStr[contentStr.Length - 1] == '\n';
            using (var contentReader = new StringReader(contentStr))
            {
                string line;
                while ((line = contentReader.ReadLine()) != null)
                {
                    var needsPara = renderer.IsRenderingBlock == false;
                    if (needsPara)
                        renderer.BeginRenderBlock(GetLineBlockType(line), null);
                    if (line.Length > 0)
                        RenderLine(line, renderer);
                    if (contentReader.Peek() > -1 || contentEnds)
                        renderer.EndRenderBlock();
                }
            }
        }

        protected virtual BlockType GetLineBlockType(string line)
        {
            return BlockType.Paragraph;
        }

        protected virtual void RenderLine(string line, IRenderer renderer)
        {
            renderer.RenderContent(line);
        }
    }
}