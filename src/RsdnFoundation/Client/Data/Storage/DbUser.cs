namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SQLite;

    [Table("User")]
    internal class DbUser
    {
        [PrimaryKey]
        public int Id { get; set; }

        [NotNull]
        public string Username { get; set; }
    }
}