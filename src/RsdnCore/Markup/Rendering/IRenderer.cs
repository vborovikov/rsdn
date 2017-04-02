namespace Rsdn.Markup.Rendering
{
    public interface IRenderer
    {
        bool IsRenderingBlock { get; }

        bool IsRenderingSection { get; }

        void BeginRender();

        void EndRender();

        void BeginRenderSection(BlockType blockType, Tag tag);

        void EndRenderSection();

        void BeginRenderBlock(BlockType blockType, Tag tag);

        void EndRenderBlock();

        void BeginRenderInline(InlineType inlineType, Tag tag);

        void EndRenderInline();

        void RenderContent(string content);

        void RenderContent(string content, ContentType contentType);
    }
}