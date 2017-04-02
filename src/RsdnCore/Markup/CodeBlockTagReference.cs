namespace Rsdn.Markup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Rendering;

    public class CodeBlockTagReference : BlockTagReference, ICodeBlockTagReference
    {
        private static readonly Dictionary<CodeLanguage, HashSet<string>> keywordsDictionary = new Dictionary<CodeLanguage, HashSet<string>>
        {
            {
                CodeLanguage.CPlusPlus,
                new HashSet<string>
                {
                    "alignas", "alignof", "and", "and_eq", "asm", "atomic_cancel", "atomic_commit", "atomic_noexcept", "auto",
                    "bitand", "bitor", "bool", "break", "case", "catch", "char", "char16_t", "char32_t", "class", "compl",
                    "concept", "const", "const_cast", "constexpr", "continue", "decltype", "default", "delete", "do", "double",
                    "dynamic_cast", "else", "enum", "explicit", "export", "extern", "false", "final", "float", "for", "friend",
                    "goto", "if", "import", "inline", "int", "long", "module", "mutable", "namespace", "new", "noexcept", "not",
                    "not_eq", "nullptr", "operator", "or", "or_eq", "override", "private", "protected", "public", "register",
                    "reinterpret_cast", "requires", "return", "short", "signed", "sizeof", "static", "static_assert", "static_cast",
                    "struct", "switch", "synchronized", "template", "this", "thread_local", "throw", "transaction_safe",
                    "transaction_safe_dynamic", "true", "try", "typedef", "typeid", "typename", "union", "unsigned", "using",
                    "virtual", "void", "volatile", "wchar_t", "while", "xor", "xor_eq",
                }
            },
            {
                CodeLanguage.CSharp,
                new HashSet<string>
                {
                    "abstract","add","alias","as","ascending","async","await","base","bool","break","by","byte","case","catch",
                    "char","checked","class","const","continue","decimal","default","delegate","descending","do","double",
                    "dynamic","else","enum","event","explicit","extern","false","finally","fixed","float","for","foreach",
                    "from","get","global","goto","group","if","implicit","in","int","interface","internal","into","is","join",
                    "let","lock","long","namespace","new","null","object","operator","orderby","out","override","params",
                    "partial","private","protected","public","readonly","ref","remove","return","sbyte","sealed","select",
                    "set","short","sizeof","stackalloc","static","string","struct","switch","this","throw","true","try","typeof",
                    "uint","ulong","unchecked","unsafe","ushort","using","value","var","virtual","void","volatile","where",
                    "while","yield",
                }
            },
            {
                CodeLanguage.Java,
                new HashSet<string>
                {
                    "abstract", "assert", "boolean", "break", "byte", "case", "catch", "char", "class", "const", "continue",
                    "default", "do", "double", "else", "enum", "extends", "final", "finally", "float", "for", "goto", "if",
                    "implements", "import", "instanceof", "int", "interface", "long", "native", "new", "package", "private",
                    "protected", "public", "return", "short", "static", "strictfp", "super", "switch", "synchronized", "this",
                    "throw", "throws", "transient", "try", "void", "volatile", "while",
                }
            }
        };

        private readonly HashSet<string> keywords;

        public CodeBlockTagReference(string tagName, CodeLanguage language)
            : base(tagName)
        {
            this.Block = BlockType.Code;
            this.IsSection = true;
            this.Language = language;

            keywordsDictionary.TryGetValue(this.Language, out this.keywords);
        }

        public CodeLanguage Language { get; }

        public bool IsKeyword(string text)
        {
            return this.keywords?.Contains(text) ?? false;
        }
    }
}