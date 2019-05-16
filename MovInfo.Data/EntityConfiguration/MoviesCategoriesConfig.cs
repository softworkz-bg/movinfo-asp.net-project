using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovInfo.Models;

namespace MovInfo.Data.EntityConfiguration
{
    public class MoviesCategoriesConfig : IEntityTypeConfiguration<MoviesCategories>
    {
        public void Configure(EntityTypeBuilder<MoviesCategories> builder)
        {
            builder.HasKey(key => new { key.MovieId, key.CategoryId });

            builder.HasOne(mc => mc.Movie)
                .WithMany(c => c.MoviesCategories)
                .HasForeignKey(key => key.MovieId);

            builder.HasOne(cm => cm.Category)
                .WithMany(m => m.MovieCategories)
                .HasForeignKey(key => key.CategoryId);
        }
    }
}