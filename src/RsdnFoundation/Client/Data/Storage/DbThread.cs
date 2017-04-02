namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SQLite;

    [Table("Thread")]
    internal class DbThread
    {
        [PrimaryKey]
        public int ThreadId { get; set; }

        public double? DisplayOffset { get; set; }

        [NotNull]
        public int PostCount { get; set; }

        [NotNull]
        public int NewPostCount { get; set; }

        public DateTime? Viewed { get; set; }
    }
}