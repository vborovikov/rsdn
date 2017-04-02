namespace Rsdn.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    [DebuggerDisplay("{ToMarkup()}", Type = "SpecialContent")]
    public class SpecialContent : Content
    {
        public SpecialContent(SpecialContentReference reference, string value)
            : base(reference, value)
        {
        }

        public sealed override bool IsPlainText => false;

        internal sealed override bool TryConcat(Content content)
        {
            return false;
        }
    }
}