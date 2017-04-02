namespace Rsdn.Markup.Rendering
{
    public enum InlineType
    {
        Bold,
        Italic,
        StrikeThrough,
        Underline,
        Hyperlink, // here come the limitation that is you can put only Run inside of Hyperlink
        Image,
        PhoneNumber,
        Command, // calls back to execute an action (e.g. a link to another thread post)
        Code,
        CodeKeyword,
        Span,
    }
}