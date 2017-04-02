namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SQLite;

    [Table("Rating")]
    internal class DbRating
    {
        [NotNull]
        public int PostId { get; set; }

        [NotNull]
        public int ThreadId { get; set; }

        [NotNull]
        public int UserId { get; set; }

        [NotNull]
        public int UserFactor { get; set; }

        [NotNull]
        public int Value { get; set; }

        [NotNull]
        public DateTime Rated { get; set; }
    }
}