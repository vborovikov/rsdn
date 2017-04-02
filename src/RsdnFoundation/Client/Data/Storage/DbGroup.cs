namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SQLite;

    [Table("Group")]
    internal class DbGroup
    {
        [PrimaryKey]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public int SortOrder { get; set; }
    }
}