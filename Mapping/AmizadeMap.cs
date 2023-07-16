using Microsoft.EntityFrameworkCore;
using HatersRating.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatersRating.Mapping
{
    public class AmizadeMap : IEntityTypeConfiguration<Amizade>
    {
        public void Configure(EntityTypeBuilder<Amizade> builder)
        {
            builder.ToTable("Amizade");

            builder.HasKey(p => p.UsuarioId);

            builder.HasKey(p => p.AmigoId);
        }
    }
}