namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Post")]
    internal class DbPost
    {
        [Key]
        public int Id { get; set; }

        public int? ThreadId { get; set; }

        public int? SubthreadId { get; set; }

        [Required]
        public int ForumId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public DateTime Posted { get; set; }

        public DateTime? Updated { get; set; }

        [Required]
        public bool IsClosed { get; set; }

        public static DateTime MostRecent(DateTime? updated, DateTime posted)
        {
            if (updated == null)
                return posted;

            return updated.Value > posted ? updated.Value : posted;
        }
    }
}