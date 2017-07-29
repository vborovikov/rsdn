namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Forum")]
    internal class DbForum
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public bool IsFavorite { get; set; }

        public DateTime? Fetched { get; set; }

        public DateTime? Posted { get; set; }

        public int? PostCount { get; set; }

        public DateTime? Visited { get; set; }
    }
}