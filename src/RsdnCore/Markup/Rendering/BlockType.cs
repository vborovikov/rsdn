namespace Rsdn.Markup.Rendering
{
    public enum BlockType
    {
        Section,
        Paragraph, // a line followed by two line breaks
        List,
        ListItem,
        Header,
        Cite1, // it's a Paragraph
        Cite2,
        Cite3,
        Cite4,
        Quote, // it's a Section
        Footnote, // [moderator] or [tagline]
        Cut,
        Code,
        HorizontalLine,
    }
}