namespace Rsdn.Markup.Rendering
{
    using System.Collections.Generic;

    public abstract class RendererBase<TBlock, TInline> : IRenderer
        where TBlock : class
        where TInline : class
    {
        private struct BlockInfo
        {
            public static readonly BlockInfo Empty = new BlockInfo();

            internal BlockInfo(BlockType blockType, TBlock block, Tag tag, bool isSection)
                : this()
            {
                this.BlockType = blockType;
                this.Block = block;
                this.Tag = tag;
                this.IsSection = isSection;
            }

            internal BlockType BlockType { get; private set; }
            internal TBlock Block { get; private set; }
            internal Tag Tag { get; private set; }
            internal bool IsSection { get; private set; }
        }

        private struct InlineInfo
        {
            internal InlineInfo(InlineType inlineType, TInline inline, Tag tag)
                : this()
            {
                this.InlineType = inlineType;
                this.Inline = inline;
                this.Tag = tag;
            }

            internal InlineType InlineType { get; private set; }
            internal TInline Inline { get; private set; }
            internal Tag Tag { get; private set; }
        }

        private readonly Stack<BlockInfo> blockStack;
        private readonly Stack<InlineInfo> inlineStack;

        protected RendererBase()
        {
            this.blockStack = new Stack<BlockInfo>();
            this.inlineStack = new Stack<InlineInfo>();
        }

        public bool IsRenderingBlock
        {
            get
            {
                if (this.blockStack.Count > 0)
                {
                    var blockInfo = this.blockStack.Peek();
                    return blockInfo.IsSection == false;
                }

                return false;
            }
        }

        public bool IsRenderingSection
        {
            get
            {
                if (this.blockStack.Count > 0)
                {
                    var blockInfo = this.blockStack.Peek();
                    return blockInfo.IsSection;
                }

                return false;
            }
        }

        public virtual void BeginRender()
        {
        }

        public virtual void EndRender()
        {
            while (this.inlineStack.Count > 0)
            {
                EndRenderInline();
            }
            while (this.blockStack.Count > 0)
            {
                EndRenderBlockCore(removeSections: true);
            }

            this.inlineStack.Clear();
            this.blockStack.Clear();
        }

        public void BeginRenderSection(BlockType blockType, Tag tag)
        {
            BeginRenderBlockCore(blockType, tag, isSection: true);
        }

        public virtual void EndRenderSection()
        {
            while (this.blockStack.Count > 0)
            {
                var blockInfo = EndRenderBlockCore(removeSections: true);
                if (blockInfo.IsSection)
                    break;
            }
        }

        public void BeginRenderBlock(BlockType blockType, Tag tag)
        {
            BeginRenderBlockCore(blockType, tag, isSection: false);
        }

        public void EndRenderBlock()
        {
            EndRenderBlockCore(removeSections: false);
        }

        public void BeginRenderInline(InlineType inlineType, Tag tag)
        {
#if PRINT_RENDER
            System.Diagnostics.Debug.WriteLine("BeginRenderInline({0}, {1}, \"{2}\")",
                inlineType, tag?.Name, tag?.Value ?? "<no value>");
#endif

            var inline = CreateInline(inlineType, tag);
            var inlineInfo = new InlineInfo(inlineType, inline, tag);
            if (this.inlineStack.Count == 0 ||
                AddInline(this.inlineStack.Peek().Inline, inlineInfo.Inline) == false)
            {
                if (this.blockStack.Count == 0)
                {
                    // There was no content before this tag on a new line, so create a new paragraph implicitly
                    BeginRenderBlock(BlockType.Paragraph, null);
                }

                AddInline(this.blockStack.Peek().Block, inlineInfo.Inline);
            }
            this.inlineStack.Push(inlineInfo);
        }

        public void EndRenderInline()
        {
            var inlineInfo = this.inlineStack.Pop();

#if PRINT_RENDER
            System.Diagnostics.Debug.WriteLine("EndRenderInline({0}, {1}, \"{2}\")",
                inlineInfo.InlineType, inlineInfo.Tag?.Name, inlineInfo.Tag?.Value ?? "<no value>");
#endif
        }

        public void RenderContent(string content)
        {
            RenderContent(content, ContentType.Text);
        }

        public void RenderContent(string content, ContentType contentType)
        {
#if PRINT_RENDER
            System.Diagnostics.Debug.WriteLine("RenderContent(\"{0}\", {1})", content, contentType);
#endif

            var run = CreateRun(content, contentType);
            if (this.inlineStack.Count == 0 || AddInline(this.inlineStack.Peek().Inline, run) == false)
            {
                AddInline(this.blockStack.Peek().Block, run);
            }
        }

        protected abstract TBlock CreateBlock(BlockType blockType, Tag tag);

        protected abstract TInline CreateInline(InlineType inlineType, Tag tag);

        protected abstract TInline CreateRun(string text, ContentType contentType);

        protected abstract void AddBlock(TBlock block);

        protected abstract bool AddBlock(TBlock block, TBlock nestedBlock);

        protected abstract bool AddInline(TBlock block, TInline inline);

        protected abstract bool AddInline(TInline inline, TInline nestedInline);

        private void BeginRenderBlockCore(BlockType blockType, Tag tag, bool isSection)
        {
#if PRINT_RENDER
            System.Diagnostics.Debug.WriteLine("{0}({1}, {2})",
                isSection ? "BeginRenderSection" : "BeginRenderBlock",
                blockType, tag != null ? tag.Name : "<content>");
#endif

            var block = CreateBlock(blockType, tag);
            var blockInfo = new BlockInfo(blockType, block, tag, isSection);
            if (this.blockStack.Count == 0 ||
                AddBlock(this.blockStack.Peek().Block, blockInfo.Block) == false)
            {
                AddBlock(blockInfo.Block);
            }
            this.blockStack.Push(blockInfo);

            var prevInlines = this.inlineStack.ToArray();
            this.inlineStack.Clear();
            foreach (var inlineInfo in prevInlines)
            {
                BeginRenderInline(inlineInfo.InlineType, inlineInfo.Tag);
            }
        }

        private BlockInfo EndRenderBlockCore(bool removeSections)
        {
            if (this.blockStack.Count == 0)
                return BlockInfo.Empty;

            var blockInfo = this.blockStack.Peek();
            var mustPop = blockInfo.IsSection == false || removeSections;
            if (mustPop)
            {
                this.blockStack.Pop();

#if PRINT_RENDER
                System.Diagnostics.Debug.WriteLine("{0}({1}, {2})",
                    blockInfo.IsSection ? "EndRenderSection" : "EndRenderBlock",
                    blockInfo.BlockType, blockInfo.Tag != null ? blockInfo.Tag.Name : "<content>");
#endif
            }

            return blockInfo;
        }
    }
}