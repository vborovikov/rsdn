namespace Rsdn.Markup.Primitives
{
    using System;

    internal sealed class OpeningTag : PlainText
    {
        internal OpeningTag(string name, string parameter = null, bool isClosed = false)
            : base(name)
        {
            this.Value = parameter;
            this.IsSelfClosing = isClosed;
        }

        public override bool IsPlainText
        {
            get
            {
                return false;
            }
        }

        public bool IsSelfClosing { get; private set; }
        internal string Name { get { return base.ToString(); } }

        internal string Value { get; private set; }

        public override string ToString()
        {
            return String.Concat("[",
                this.Name,
                String.IsNullOrWhiteSpace(this.Value) ? String.Empty : "=" + this.Value,
                this.IsSelfClosing ? "/]" : "]");
        }
    }
}