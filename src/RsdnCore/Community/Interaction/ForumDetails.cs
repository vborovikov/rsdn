namespace Rsdn.Community.Interaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ForumDetails : IIdentifiable
    {
        public static readonly ForumDetails Empty = new ForumDetails
        {
            Id = -1,
            Name = String.Empty,
            ShortName = String.Empty
        };

        public int Id { get; private set; }

        public string ShortName { get; private set; }

        public string Name { get; private set; }

        public bool IsFavorite { get; set; }

        public int? PostCount { get; private set; }

        public DateTime? Posted { get; private set; }

        public DateTime? Visited { get; private set; }
    }
}