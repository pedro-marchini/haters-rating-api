namespace HatersRating.Models
{
    public class Amizade : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public Guid AmigoId { get; set; }
    }
}