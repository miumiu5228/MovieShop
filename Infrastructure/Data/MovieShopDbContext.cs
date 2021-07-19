using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MovieShopDbContext : DbContext
    {
        public MovieShopDbContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<MovieCrew> MovieCrews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<MovieCast> MovieCasts { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Role> Roles { get; set; }

        // to use fluent API we need to override a nmethod OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Movie>(ConfigureMovie);
            modelBuilder.Entity<Trailer>(ConfigureTrailer);
            modelBuilder.Entity<MovieCrew>(ConfigureMovieCrew);
            modelBuilder.Entity<Crew>(ConfigureCrew);
            modelBuilder.Entity<MovieCast>(ConfigureMovieCast);
            modelBuilder.Entity<Cast>(ConfigureCast);
            modelBuilder.Entity<Review>(ConfigureReview);
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<Favorite>(ConfigureFavorite);
            modelBuilder.Entity<Purchase>(ConfigurePurchase);
            modelBuilder.Entity<Role>(ConfigureRole);

            //Creating junction table
            modelBuilder.Entity<Movie>(ConfigureMovie);
            modelBuilder.Entity<Movie>().HasMany(m => m.Genres).WithMany(g => g.Movies)
                .UsingEntity<Dictionary<string, object>>("MovieGenre",
                 m => m.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                 g => g.HasOne<Movie>().WithMany().HasForeignKey("MovieId"));



            modelBuilder.Entity<User>().HasMany(u => u.Roles).WithMany(r => r.Users)
               .UsingEntity<Dictionary<string, Object>>("UserRole",
               u => u.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
               r => r.HasOne<User>().WithMany().HasForeignKey("UserId"));

        }
        private void ConfigureReview(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Review");
            builder.HasKey(r => new { r.MovieId, r.UserId });
            builder.HasOne(r => r.Movie).WithMany(r => r.Reviews).HasForeignKey(r => r.MovieId);
            builder.HasOne(r => r.User).WithMany(r => r.Reviews).HasForeignKey(r => r.UserId);
            builder.Property(r => r.Rating).HasColumnType("decimal(3, 2)").HasDefaultValue(9.9m);
            builder.Property(r => r.ReviewText).HasMaxLength(4096);
        }
        private void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName).HasMaxLength(128);
            builder.Property(u => u.LastName).HasMaxLength(128);
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.HashedPassword).HasMaxLength(1024);
            builder.Property(u => u.Salt).HasMaxLength(1024);
            builder.Property(u => u.PhoneNumber).HasMaxLength(16);
        }

        private void ConfigureRole(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(20);
        }
        private void ConfigurePurchase(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("Purchase");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.TotalPrice).HasColumnType("decimal(18, 2)").HasDefaultValue(9.9m);

            //The Foreign key for the corresponding column(optional)
            //Using when you want select the coulum name.
            builder.HasOne(mu => mu.Movie).WithMany(mu => mu.Purchases).HasForeignKey(mu => mu.MovieId);
            builder.HasOne(mu => mu.User).WithMany(mu => mu.Purchases).HasForeignKey(mu => mu.UserId);
        }
        private void ConfigureFavorite(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorite");
            builder.HasKey(t => t.Id);
            //The Foreign key for the corresponding column(optional)
            //Using when you want select the coulum name.
            builder.HasOne(mu => mu.Movie).WithMany(mu => mu.Favorites).HasForeignKey(mu => mu.MovieId);
            builder.HasOne(mu => mu.User).WithMany(mu => mu.Favorites).HasForeignKey(mu => mu.UserId);
        }
        private void ConfigureCast(EntityTypeBuilder<Cast> builder)
        {
            builder.ToTable("Cast");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(128);
            builder.Property(t => t.Gender).HasMaxLength(4096);
            builder.Property(t => t.TmdbUrl).HasMaxLength(4096);
            builder.Property(t => t.ProfilePath).HasMaxLength(2084);
        }

        private void ConfigureMovieCast(EntityTypeBuilder<MovieCast> builder)
        {
            builder.ToTable("MovieCast");
            
            //Mutiple primary key in junction table
            builder.HasKey(mc=>new { mc.CastId, mc.MovieId, mc.Character});
            
            //The Foreign key for the corresponding column(optional)
            //Using when you want select the coulum name.
            builder.HasOne(mc => mc.Movie).WithMany(mc => mc.MovieCasts).HasForeignKey(mc => mc.MovieId);
            builder.HasOne(mc => mc.Cast).WithMany(mc => mc.MovieCasts).HasForeignKey(mc => mc.CastId);

            builder.Property(t => t.Character).HasMaxLength(450);
        }
        private void ConfigureCrew(EntityTypeBuilder<Crew> builder)
        {
            builder.ToTable("Crew");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(128);
            builder.Property(t => t.Gender).HasMaxLength(4096);
            builder.Property(t => t.TmdbUrl).HasMaxLength(4096);
            builder.Property(t => t.ProfilePath).HasMaxLength(2084);
        }
        private void ConfigureMovieCrew(EntityTypeBuilder<MovieCrew> builder)
        {
            builder.ToTable("MovieCrew");
            builder.HasKey(mc => new { mc.MovieId, mc.CrewId, mc.Department,mc.Job });
            //The Foreign key for the corresponding column(optional)
            //Using when you want select the coulum name.
            builder.HasOne(mc => mc.Movie).WithMany(mc => mc.MovieCrews).HasForeignKey(mc => mc.MovieId);
            builder.HasOne(mc => mc.Crew).WithMany(mc => mc.MovieCrews).HasForeignKey(mc => mc.CrewId);
            
            builder.Property(t => t.Department).HasMaxLength(128);
            builder.Property(t => t.Job).HasMaxLength(128);
        }
        private void ConfigureTrailer(EntityTypeBuilder<Trailer> builder)
        {
            builder.ToTable("Trailer");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.TrailerUrl).HasMaxLength(2084);
            builder.Property(t => t.Name).HasMaxLength(2084);
        }
        private void ConfigureMovie(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movie");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Title).HasMaxLength(256).IsRequired();
            builder.Property(m => m.Overview).HasMaxLength(4096);
            builder.Property(m => m.Tagline).HasMaxLength(512);
            builder.Property(m => m.ImdbUrl).HasMaxLength(2084);
            builder.Property(m => m.TmdbUrl).HasMaxLength(2084);
            builder.Property(m => m.PosterUrl).HasMaxLength(2084);
            builder.Property(m => m.BackdropUrl).HasMaxLength(2084);
            builder.Property(m => m.OriginalLanguage).HasMaxLength(64);
            builder.Property(m => m.Price).HasColumnType("decimal(5, 2)").HasDefaultValue(9.9m);
            builder.Property(m => m.Budget).HasColumnType("decimal(18, 4)").HasDefaultValue(9.9m);
            builder.Property(m => m.Revenue).HasColumnType("decimal(18, 4)").HasDefaultValue(9.9m);
            builder.Property(m => m.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Ignore(m => m.Rating);
        }
    }
}
