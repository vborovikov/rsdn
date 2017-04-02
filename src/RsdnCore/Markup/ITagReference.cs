namespace Rsdn.Markup
{
    public interface ITagReference
    {
        string Name { get; }

        bool ContentsAreValue { get; }

        bool RequiresClosingTag { get; }
    }
}