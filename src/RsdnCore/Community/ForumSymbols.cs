namespace Rsdn.Community
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public struct ForumSymbols
    {
        public readonly int ForumId;
        public readonly string ShortSymbol;
        public readonly string MediumSymbol;

        internal ForumSymbols(int forumId, string shortSymbol, string mediumSymbol)
        {
            this.ForumId = forumId;
            this.ShortSymbol = shortSymbol;
            this.MediumSymbol = mediumSymbol;
        }
    }
}