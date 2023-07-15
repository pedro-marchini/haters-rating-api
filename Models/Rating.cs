using System;

namespace HatersRating.Models
{
    class Rating
    {
        public Guid Id { get; set; }
        public required string Titulo { get; set; }
        public string? Descricao { get; set; }
        public uint Rate { get; set; }
        public string? Imagem { get; set; }

    }

}