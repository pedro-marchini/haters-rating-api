namespace HatersRating.Models
{
    public class UsuariosRating : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public Guid RatingId { get; set; }
    }
}