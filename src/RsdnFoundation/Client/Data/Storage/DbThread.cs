namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Thread")]
    internal class DbThread
    {
        [Key]
        public int ThreadId { get; set; }

        public double? DisplayOffset { get; set; }

        [Required]
        public int PostCount { get; set; }

        [Required]
        public int NewPostCount { get; set; }

        public DateTime? Viewed { get; set; }
    }
}