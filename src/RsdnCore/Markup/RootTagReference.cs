namespace Rsdn.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Rendering;

    internal sealed class RootTagReference : TagReference
    {
        internal RootTagReference()
            : base(null)
        {
            this.RequiresClosingTag = true;
        }

        protected override void Render(Tag tag, IRenderer renderer)
        {
            renderer.BeginRender();
            base.Render(tag, renderer);
            renderer.EndRender();
        }
    }
}