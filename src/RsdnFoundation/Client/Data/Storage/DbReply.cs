namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SQLite;

    [Table("Reply")]
    internal class DbReply
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int? PostId { get; set; }

        [NotNull]
        public int ForumId { get; set; }

        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string Message { get; set; }
    }
}