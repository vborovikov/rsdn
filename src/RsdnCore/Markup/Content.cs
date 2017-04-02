namespace Rsdn.Markup
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Rendering;

    /// <summary>
    /// Represents single piece of content. Never empty, at least contains a line break symbol.
    /// </summary>
    [DebuggerDisplay("{ToMarkup()}", Type = "Content")]
    public class Content
    {
        private readonly ContentReference reference;

        internal Content(ContentReference reference, string value)
        {
            this.reference = reference;
            this.Value = value;
        }

        public virtual bool IsPlainText => true;

        public virtual string Value { get; private set; }

        public ContentPosition Position => this.Owner?.GetContentPosition(this) ?? ContentPosition.None;

        internal Tag Owner { get; set; }

        public override string ToString()
        {
            var str = this.Value ?? String.Empty;

            if (this.Owner != null && this.Owner.IsBlock)
            {
                var pos = this.Position;
                if (pos == ContentPosition.First || pos == ContentPosition.Single ||
                    ((this.Owner.ContentBefore(this) as Tag)?.IsBlock ?? false))
                {
                    // eat first line in/after block
                    var firstLnIdx = str.IndexOf('\n');
                    if (firstLnIdx > -1)
                    {
                        var firstLine = str.Substring(0, firstLnIdx + 1);
                        if (String.IsNullOrWhiteSpace(firstLine))
                        {
                            str = str.Remove(0, firstLine.Length);
                        }
                    }
                }
                else if (pos == ContentPosition.Last || pos == ContentPosition.Single)
                {
                    // eat last line in block
                    var lastLnIdx = str.LastIndexOf('\n');
                    if (lastLnIdx > -1)
                    {
                        var lastLine = str.Substring(lastLnIdx);
                        if (String.IsNullOrWhiteSpace(lastLine))
                        {
                            str = str.Remove(lastLnIdx);
                        }
                    }
                }
            }

            return str;
        }

        public virtual string ToMarkup()
        {
            return this.Value ?? String.Empty;
        }

        public void Render(IRenderer renderer)
        {
            this.reference.Render(this, renderer);
        }

        internal virtual bool TryConcat(Content content)
        {
            if (content.IsPlainText)
            {
                this.Value = this.Value + content.ToMarkup();
                return true;
            }

            return false;
        }
    }
}