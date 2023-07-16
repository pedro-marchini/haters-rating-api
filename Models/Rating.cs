using System;

namespace HatersRating.Models
{
    public class Rating : EntityBase
    {
        public required string Titulo { get; set; }
        public string? Descricao { get; set; }
        public uint Rate { get; set; }
        public string? Imagem { get; set; }
    }
}