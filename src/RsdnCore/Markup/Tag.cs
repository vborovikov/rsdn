namespace Rsdn.Markup
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Rendering;

    [DebuggerDisplay("{ToMarkup()}", Type = "[{Name}/]")]
    public class Tag : Content, IEnumerable<Content>
    {
        private readonly TagReference reference;
        private readonly List<Content> contents;

        internal Tag(TagReference reference, string value = null)
            : base(reference, value)
        {
            this.reference = reference;
            this.contents = new List<Content>();
        }

        public override bool IsPlainText => false;

        public ITagReference Reference => this.reference;

        public override string Value => this.reference.ExpandValue(this, this.reference.ContentsAreValue ? ToString() : base.Value);

        public bool TreatsContentsAsValue => this.reference.ContentsAreValue;

        public string Name => this.reference.Name;

        public bool RequiresClosingTag => this.reference.RequiresClosingTag;

        public virtual bool IsBlock => this.reference is BlockTagReference;

        public bool IsCode => (this.reference as BlockTagReference)?.Block == BlockType.Code;

        public void Add(Content content)
        {
            if (TryConcatContent(content) == false)
            {
                this.contents.Add(content);
                content.Owner = this;
            }
        }

        public Tag FindTag(Predicate<Tag> match)
        {
            foreach (var item in this.contents)
            {
                var tag = item as Tag;
                if (tag == null)
                    continue;
                if (match(tag))
                    return tag;
                tag = tag.FindTag(match);
                if (tag != null)
                    return tag;
            }

            return null;
        }

        public override string ToString()
        {
            return String.Concat(this.contents);
        }

        public override string ToMarkup()
        {
            var markupRepresentation = new StringBuilder();

            markupRepresentation
                .Append("[")
                .Append(this.Name);
            if (String.IsNullOrWhiteSpace(this.Value) == false)
            {
                markupRepresentation
                    .Append("=")
                    .Append(this.Value);
            }
            markupRepresentation.Append("]");

            foreach (var content in this.contents)
            {
                markupRepresentation.Append(content.ToMarkup());
            }

            if (this.RequiresClosingTag)
            {
                markupRepresentation
                    .Append("[/")
                    .Append(this.Name)
                    .Append("]");
            }

            return markupRepresentation.ToString();
        }

        public IEnumerator<Content> GetEnumerator()
        {
            return this.contents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal Content ContentBefore(Content content)
        {
            var idx = this.contents.IndexOf(content);
            if (idx <= 0)
                return null;
            return this.contents.ElementAt(idx - 1);
        }

        internal sealed override bool TryConcat(Content content)
        {
            return false;
        }

        internal ContentPosition GetContentPosition(Content content)
        {
            var contentIndex = this.contents.IndexOf(content);
            var lastIndex = this.contents.Count - 1;
            return
                contentIndex < 0 ? ContentPosition.None :
                contentIndex == 0 ? (contentIndex == lastIndex ? ContentPosition.Single : ContentPosition.First) :
                contentIndex == lastIndex ? ContentPosition.Last : ContentPosition.Middle;
        }

        private bool TryConcatContent(Content content)
        {
            var prevContent = this.contents.LastOrDefault();
            if (prevContent != null)
            {
                return prevContent.TryConcat(content);
            }

            return false;
        }
    }
}