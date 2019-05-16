using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovInfo.Models;

namespace MovInfo.Data.EntityConfiguration
{
    public class MoviesActorsConfig : IEntityTypeConfiguration<MoviesActors>
    {
        public void Configure(EntityTypeBuilder<MoviesActors> builder)
        {
            builder.HasKey(key => new { key.MovieId, key.ActorId });

            builder.HasOne(ma => ma.Movie)
                .WithMany(a => a.MoviesActors)
                .HasForeignKey(key => key.MovieId);

            builder.HasOne(am => am.Actor)
                .WithMany(m => m.MoviesActors)
                .HasForeignKey(key => key.ActorId);
        }
    }
}