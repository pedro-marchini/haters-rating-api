using HatersRating.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HatersRating.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Senha)
                .IsRequired()
                .HasColumnType("varchar(200)");

        }
    }
}