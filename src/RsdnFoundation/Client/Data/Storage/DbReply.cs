namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Reply")]
    internal class DbReply
    {
        [Key]
        public int Id { get; set; }

        public int? PostId { get; set; }

        [Required]
        public int ForumId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }
    }
}