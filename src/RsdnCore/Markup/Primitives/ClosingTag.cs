namespace Rsdn.Markup.Primitives
{
    using System;

    internal sealed class ClosingTag : PlainText
    {
        internal ClosingTag(string name)
            : base(name)
        {
        }

        public override bool IsPlainText
        {
            get
            {
                return false;
            }
        }

        internal string Name { get { return base.ToString(); } }

        public override string ToString()
        {
            return String.Concat("[/", this.Name, "]");
        }
    }
}