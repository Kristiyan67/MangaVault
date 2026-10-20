using MangaVault.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaVault.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<SeriesGenre> SeriesGenres => Set<SeriesGenre>();
        public DbSet<Series> Series => Set<Series>();
        public DbSet<Volume> Volume => Set<Volume>();
        public DbSet<Author> Author => Set<Author>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Genre>(e =>
            {
                e.HasIndex(g => g.Name).IsUnique();
                e.HasData(
                   new Genre { Id = 1, Name = "Shonen" },
                   new Genre { Id = 2, Name = "Seinen" },
                   new Genre { Id = 3, Name = "Shojo" },
                   new Genre { Id = 4, Name = "Isekai" });
            });

            builder.Entity<SeriesGenre>(e =>
            {
                e.HasKey(sg => new { sg.SeriesId, sg.GenreId });

                e.HasOne(sg => sg.Genre)
                .WithMany(g => g.SeriesGenres)
                .HasForeignKey(sg => sg.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

                // Series chast s delete behavior
            });

            builder.Entity<Volume>(e =>
            {
                e.HasIndex(v => new { v.SeriesId, v.Number })
                .IsUnique();
            });
        }
    }
}
