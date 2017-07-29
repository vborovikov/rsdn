namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    internal class RsdnDbContext : DbContext
    {
        static RsdnDbContext()
        {
            using (var ctx = new RsdnDbContext())
            {
                //todo: use migrations
                ctx.Database.EnsureCreated();
            }
        }

        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbGroup> Groups { get; set; }
        public DbSet<DbForum> Forums { get; set; }
        public DbSet<DbThread> Threads { get; set; }
        public DbSet<DbPost> Posts { get; set; }
        public DbSet<DbRating> Ratings { get; set; }
        public DbSet<DbReply> Replies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=rsdn.dat");
            optionsBuilder.UseSqliteLolita();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // auto-increment
            modelBuilder.Entity<DbReply>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            // composite key
            modelBuilder.Entity<DbRating>()
                .HasKey(nameof(DbRating.PostId), nameof(DbRating.UserId), nameof(DbRating.Value));
        }
    }
}