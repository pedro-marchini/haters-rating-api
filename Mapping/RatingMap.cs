using Microsoft.EntityFrameworkCore;
using HatersRating.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatersRating.Mapping
{
    public class RatingMap : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Rating");

            builder.Property(p => p.Titulo)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.Descricao)
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Rate)
                .IsRequired()
                .HasColumnType("tinyint");

            builder.Property(p => p.Imagem)
                .HasColumnType("varchar(100)");
        }
    }
}