namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    internal class DbUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
    }
}