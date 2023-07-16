using HatersRating.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatersRating.Mapping
{
    public class UsuariosRatingMap : IEntityTypeConfiguration<UsuariosRating>
    {
        public void Configure(EntityTypeBuilder<UsuariosRating> builder)
        {
            builder.ToTable("UsuariosRating");

            builder.HasKey(p => p.UsuarioId);

            builder.HasKey(p => p.RatingId);
        }
    }
}