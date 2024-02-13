using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace URLShortenerWeb.Data
{
    public partial class SQLDbContext : DbContext
    {
        public SQLDbContext()
        {
        }

        public SQLDbContext(DbContextOptions<SQLDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Url> Urls { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               // optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=URLShortener;Integrated Security=true;"); defined in the program.cs class from the appsettings.json.
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>(entity =>
            {
                entity.ToTable("URLs");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastAccessed).HasColumnType("datetime");

                entity.Property(e => e.OriginalUrl).HasColumnName("OriginalURL");

                entity.Property(e => e.ShortenedUrlcode)
                    .HasMaxLength(8)
                    .HasColumnName("ShortenedURLCode");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
