using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovInfo.Data.EntityConfiguration;
using MovInfo.Models;
using System.Data.Common;

namespace MovInfo.Data
{
    public class MovInfoContext : IdentityDbContext<ApplicationUser>
    {
        private readonly string connectionString;

        public MovInfoContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.connectionString = configuration
                                    .GetSection("ConnectionStrings:DefaultConnection")
                                    .Value;
        }

        public MovInfoContext(DbContextOptions options) : base(options)
        {
        }

        public MovInfoContext()
        {
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MoviesCategories> MoviesCategories { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MoviesActorsConfig());
            modelBuilder.ApplyConfiguration(new MoviesCategoriesConfig());

            base.OnModelCreating(modelBuilder);
        }

        public DbConnection GetDbConnection()
        {
            return this.Database.GetDbConnection();
        }
    }
}