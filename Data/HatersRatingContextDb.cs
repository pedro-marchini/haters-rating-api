using Microsoft.EntityFrameworkCore;
using HatersRating.Models;

namespace HatersRating.Data
{
    class HatersRatingContextDb : DbContext
    {
        public HatersRatingContextDb(DbContextOptions<HatersRatingContextDb> options) : base(options) { }

        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Rating>()
                .Property(p => p.Titulo)
                .IsRequired()
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Rating>()
                .Property(p => p.Descricao)
                .HasColumnType("varchar(250)");

            modelBuilder.Entity<Rating>()
                .Property(p => p.Rate)
                .IsRequired()
                .HasColumnType("tinyint");

            modelBuilder.Entity<Rating>()
                .Property(p => p.Imagem)
                .HasColumnType("varchar(100)");

            modelBuilder.Entity<Rating>()
                .ToTable("Ratings");

            base.OnModelCreating(modelBuilder);
        }
    }

}