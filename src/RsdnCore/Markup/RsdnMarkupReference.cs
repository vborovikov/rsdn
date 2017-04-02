namespace Rsdn.Markup
{
    using CustomTags;
    using Rendering;

    public sealed class RsdnMarkupReference : MarkupReference
    {
        public static class KnownTags
        {
            public const string Image = "IMG";
            public const string Hyperlink = "URL";
            public const string Bold = "B";
            public const string Italic = "I";
            public const string StrikeThrough = "S";
            public const string Underline = "U";
            public const string Superscript = "SUP";
            public const string Subscript = "SUB";
            public const string Quote = "Q";
            public const string HorizontalLine = "HR";
            public const string List = "LIST";
            public const string ListItem = "*";
            public const string Email = "EMAIL";
            public const string Msdn = "MSDN";
            public const string Tagline = "TAGLINE";
            public const string Moderator = "MODERATOR";
            public const string Cut = "CUT";
            public const string Header1 = "H1";
            public const string Header2 = "H2";
            public const string Header3 = "H3";
            public const string Header4 = "H4";
            public const string Header5 = "H5";
            public const string Header6 = "H6";
            public const string Table = "TABLE";
            public const string CodeInline = "TT";
            public const string CodeBlock = "CODE";
            public const string CodeCLang = "C";
            public const string CodeCLang2 = "CCODE";
            public const string CodeMsil = "MSIL";
            public const string CodeMidl = "MIDL";
            public const string CodeCSharp = "C#";
            public const string CodeCSharp2 = "CS";
            public const string CodeCSharp3 = "CSHARP";
            public const string CodeSql = "SQL";
            public const string CodeAssembly = "ASM";
            public const string CodeJava = "JAVA";
            public const string CodeML = "ML";
            public const string CodePascal = "PASCAL";
            public const string CodeNemerle = "NEMERLE";
            public const string CodeHaskell = "HASKELL";
            public const string CodePerl = "PERL";
            public const string CodeVisualBasic = "VB";
            public const string CodePhp = "PHP";
            public const string CodeXml = "XML";
        }

        public static class CustomTags
        {
            public const string Pre = "PRE";
            public const string Skip = "SKIP";
        }

        public static readonly RsdnMarkupReference Current = new RsdnMarkupReference();

        private static readonly ContentReference CiteContentReference = new CiteContentReference();

        private RsdnMarkupReference()
        {
            Add(new InlineTagReference(KnownTags.Bold)
            {
                Inline = InlineType.Bold
            });
            Add(new InlineTagReference(KnownTags.Italic)
            {
                Inline = InlineType.Italic
            });
            Add(new InlineTagReference(KnownTags.StrikeThrough)
            {
                Inline = InlineType.StrikeThrough
            });
            Add(new InlineTagReference(KnownTags.Underline)
            {
                Inline = InlineType.Underline
            });
            Add(new BlockTagReference(KnownTags.Quote)
            {
                Block = BlockType.Quote,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.HorizontalLine)
            {
                RequiresClosingTag = false,
                Block = BlockType.HorizontalLine
            });
            Add(new ImageTagReference());

            Add(new BlockTagReference(KnownTags.List)
            {
                Block = BlockType.List,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.ListItem)
            {
                RequiresClosingTag = false,
                Block = BlockType.ListItem
            });

            Add(new EmailTagReference());
            Add(new MsdnTagReference());
            Add(new BlockTagReference(KnownTags.Tagline)
            {
                Block = BlockType.Footnote
            });
            Add(new BlockTagReference(KnownTags.Moderator)
            {
                Block = BlockType.Footnote
            });
            Add(new BlockTagReference(KnownTags.Cut)
            {
                Block = BlockType.Cut,
                IsSection = true
            });

            Add(new BlockTagReference(KnownTags.Header1)
            {
                Block = BlockType.Header
            });

            Add(new InlineTagReference(KnownTags.CodeInline)
            {
                Inline = InlineType.Code,
            });
            Add(new BlockTagReference(KnownTags.CodeBlock)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new CodeBlockTagReference(KnownTags.CodeCLang, CodeLanguage.CPlusPlus));
            Add(new CodeBlockTagReference(KnownTags.CodeCLang2, CodeLanguage.CPlusPlus));
            Add(new BlockTagReference(KnownTags.CodeMsil)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.CodeMidl)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new CodeBlockTagReference(KnownTags.CodeCSharp, CodeLanguage.CSharp));
            Add(new CodeBlockTagReference(KnownTags.CodeCSharp2, CodeLanguage.CSharp));
            Add(new CodeBlockTagReference(KnownTags.CodeCSharp3, CodeLanguage.CSharp));
            Add(new BlockTagReference(KnownTags.CodeSql)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.CodeAssembly)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new CodeBlockTagReference(KnownTags.CodeJava, CodeLanguage.Java));
            Add(new BlockTagReference(KnownTags.CodeML)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.CodePascal)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.CodeNemerle)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.CodeHaskell)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.CodePerl)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.CodeVisualBasic)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.CodePhp)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new BlockTagReference(KnownTags.CodeXml)
            {
                Block = BlockType.Code,
                IsSection = true
            });

            // Unofficial tags

            Add(new BlockTagReference(CustomTags.Pre)
            {
                Block = BlockType.Code,
                IsSection = true
            });
            Add(new SkipTagReference());
        }

        protected override Content CreateContent(string text)
        {
            return new Content(CiteContentReference, text);
        }
    }
}