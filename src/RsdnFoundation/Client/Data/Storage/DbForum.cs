namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SQLite;

    [Table("Forum")]
    internal class DbForum
    {
        [PrimaryKey]
        public int Id { get; set; }

        [NotNull]
        public int GroupId { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string ShortName { get; set; }

        [NotNull]
        public bool IsFavorite { get; set; }

        public DateTime? Fetched { get; set; }

        public DateTime? Posted { get; set; }

        public int? PostCount { get; set; }

        public DateTime? Visited { get; set; }
    }
}