namespace Rsdn.Markup
{
    using System;
    using System.Linq;
    using Rsdn.Markup.Rendering;

    internal sealed class CiteContentReference : ContentReference
    {
        public const char CiteChar = '>';

        public static int FindCiteIndex(string para)
        {
            var citeIndex = para.IndexOf(CiteChar);
            return citeIndex >= 0 && citeIndex <= 5 ? citeIndex : -1;
        }

        protected override BlockType GetLineBlockType(string line)
        {
            var citeIndex = FindCiteIndex(line);
            if (citeIndex > -1)
            {
                var level = line.Skip(citeIndex).Take(3).Count(ch => ch == CiteChar);
                return
                    level == 1 ? BlockType.Cite1 :
                    level == 2 ? BlockType.Cite2 :
                    level == 3 ? BlockType.Cite3 :
                    BlockType.Cite4;
            }

            return base.GetLineBlockType(line);
        }
    }
}