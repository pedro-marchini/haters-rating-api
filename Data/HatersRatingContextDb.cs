using Microsoft.EntityFrameworkCore;
using HatersRating.Models;
using HatersRating.Mapping;

namespace HatersRating.Data
{
    class HatersRatingContextDb : DbContext
    {
        public HatersRatingContextDb(DbContextOptions<HatersRatingContextDb> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuariosRating> UsuariosRating { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Amizade> Amizade { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(new UsuarioMap().Configure);
            modelBuilder.Entity<Rating>(new RatingMap().Configure);
            modelBuilder.Entity<Amizade>(new AmizadeMap().Configure);
            modelBuilder.Entity<UsuariosRating>(new UsuariosRatingMap().Configure);
        }
    }
}