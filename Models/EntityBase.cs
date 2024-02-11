namespace HatersRating.Models
{
    public class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public bool Activated { get; set; } = true;
        public bool Deleted { get; set; } = false;
    }
}