using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;

namespace MoviesAdmin.Data
{
    public class MoviesAdminContext : DbContext
    {
        public MoviesAdminContext(DbContextOptions<MoviesAdminContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default;
        public DbSet<Genre> Genres { get; set; } = default;
        public DbSet<AudienceRating> AudienceRatings { get; set; } = default;
        public DbSet<Review> Reviews { get; set; } = default;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .Property(m => m.ReleaseDate)
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v)
                );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Title = "Action" },
                new Genre { Id = 2, Title = "Comedy" },
                new Genre { Id = 3, Title = "Drama" },
                new Genre { Id = 4, Title = "Sci-Fi" },
                new Genre { Id = 5, Title = "Family" },
                new Genre { Id = 6, Title = "Horror" },
                new Genre { Id = 7, Title = "Thriller" },
                new Genre { Id = 8, Title = "Romance" },
                new Genre { Id = 9, Title = "Animation" },
                new Genre { Id = 10, Title = "Documentary" }
            );

            modelBuilder.Entity<AudienceRating>().HasData(
                new AudienceRating { Id = 1, Rating = "G" },
                new AudienceRating { Id = 2, Rating = "PG" },
                new AudienceRating { Id = 3, Rating = "PG-13" },
                new AudienceRating { Id = 4, Rating = "R" }
            );
        }
    }
}
