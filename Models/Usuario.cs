namespace HatersRating.Models
{
    public class Usuario : EntityBase
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}
