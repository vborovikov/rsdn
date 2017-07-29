namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Rating")]
    internal class DbRating
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public int ThreadId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int UserFactor { get; set; }

        [Required]
        public int Value { get; set; }

        [Required]
        public DateTime Rated { get; set; }
    }
}