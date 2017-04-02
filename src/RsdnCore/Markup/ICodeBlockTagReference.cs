namespace Rsdn.Markup
{
    public interface ICodeBlockTagReference : ITagReference
    {
        CodeLanguage Language { get; }

        bool IsKeyword(string text);
    }
}