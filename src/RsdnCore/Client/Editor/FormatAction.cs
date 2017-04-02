namespace Rsdn.Client.Editor
{
    using Markup;

    public enum FormatAction
    {
        [Tag(RsdnMarkupReference.KnownTags.Bold)]
        Bold,

        [Tag(RsdnMarkupReference.KnownTags.Italic)]
        Italic,

        [Tag(RsdnMarkupReference.KnownTags.StrikeThrough)]
        Strikethrough,

        [Tag(RsdnMarkupReference.KnownTags.Underline)]
        Underline,

        [Tag(RsdnMarkupReference.KnownTags.Superscript)]
        Superscript,

        [Tag(RsdnMarkupReference.KnownTags.Subscript)]
        Subscript,

        [Tag(RsdnMarkupReference.KnownTags.Image)]
        Image,

        [Tag(RsdnMarkupReference.KnownTags.Hyperlink)]
        Link,

        [Tag(RsdnMarkupReference.KnownTags.Header1)]
        Heading1,

        [Tag(RsdnMarkupReference.KnownTags.Header2)]
        Heading2,

        [Tag(RsdnMarkupReference.KnownTags.Header3)]
        Heading3,

        [Tag(RsdnMarkupReference.KnownTags.Header4)]
        Heading4,

        [Tag(RsdnMarkupReference.KnownTags.Header5)]
        Heading5,

        [Tag(RsdnMarkupReference.KnownTags.Header6)]
        Heading6,

        [Tag(RsdnMarkupReference.KnownTags.List)]
        List,

        [Tag(RsdnMarkupReference.KnownTags.ListItem)]
        ListItem,

        [Tag(RsdnMarkupReference.KnownTags.Table)]
        Table,

        [Tag(RsdnMarkupReference.KnownTags.Quote)]
        Quote,

        [Tag(RsdnMarkupReference.KnownTags.Cut)]
        Collapse,

        [Tag(RsdnMarkupReference.KnownTags.CodeInline)]
        CodeInline,

        [Tag(RsdnMarkupReference.KnownTags.CodeBlock)]
        CodeBlock,

        [Tag(RsdnMarkupReference.KnownTags.CodeCLang)]
        CodeC,

        [Tag(RsdnMarkupReference.KnownTags.CodeCLang2)]
        CodeCPlusPlus,

        [Tag(RsdnMarkupReference.KnownTags.CodeCSharp2)]
        CodeCSharp,

        [Tag(RsdnMarkupReference.KnownTags.CodeJava)]
        CodeJava,

        [Tag(RsdnMarkupReference.KnownTags.CodeXml)]
        CodeXml,

        [Tag(RsdnMarkupReference.KnownTags.CodeSql)]
        CodeSql,
    }
}