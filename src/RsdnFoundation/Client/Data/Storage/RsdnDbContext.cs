namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Logging;

    internal class RsdnDbContext : DbContext
    {
#if DEBUG && TRACE
        private static readonly ILoggerFactory loggerFactory;
#endif

        static RsdnDbContext()
        {
#if DEBUG && TRACE
            loggerFactory = new LoggerFactory();
            loggerFactory.AddDebug();
#endif

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

#if DEBUG && TRACE
            optionsBuilder.ConfigureWarnings(warnCfgBuilder =>
            {
                warnCfgBuilder.Log(RelationalEventId.QueryClientEvaluationWarning);
            });
            optionsBuilder.UseLoggerFactory(loggerFactory);
#endif
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