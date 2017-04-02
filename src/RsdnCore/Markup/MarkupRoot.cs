namespace Rsdn.Markup
{
    public sealed class MarkupRoot : Tag
    {
        public MarkupRoot()
            : base(MarkupReference.MarkupRootReference)
        {
        }

        public override bool IsBlock { get { return true; } }
    }
}