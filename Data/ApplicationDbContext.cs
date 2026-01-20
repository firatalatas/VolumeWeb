using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VolumeWeb.Models;

namespace VolumeWeb.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<AlbumFavorite> AlbumFavorites { get; set; }
    public DbSet<ArtistFavorite> ArtistFavorites { get; set; }
    public DbSet<AlbumRating> AlbumRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Album>()
            .HasMany(a => a.Artists)
            .WithMany(ar => ar.Albums);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Album)
            .WithMany(a => a.Reviews)
            .HasForeignKey(r => r.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        // Favorites
        modelBuilder.Entity<AlbumFavorite>()
            .HasIndex(af => new { af.UserId, af.AlbumId })
            .IsUnique();

        modelBuilder.Entity<ArtistFavorite>()
            .HasIndex(af => new { af.UserId, af.ArtistId })
            .IsUnique();

        // Ratings
        modelBuilder.Entity<AlbumRating>()
            .HasIndex(ar => new { ar.UserId, ar.AlbumId })
            .IsUnique();
    }
}
