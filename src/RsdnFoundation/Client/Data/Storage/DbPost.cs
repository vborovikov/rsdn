namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SQLite;

    [Table("Post")]
    internal class DbPost
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int? ThreadId { get; set; }

        public int? SubthreadId { get; set; }

        [NotNull]
        public int ForumId { get; set; }

        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string Message { get; set; }

        [NotNull]
        public int UserId { get; set; }

        [NotNull]
        public string Username { get; set; }

        [NotNull]
        public DateTime Posted { get; set; }

        public DateTime? Updated { get; set; }

        [NotNull]
        public bool IsClosed { get; set; }
    }
}