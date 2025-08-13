namespace Day06.API.Models
{
    public class ContentBlock: BaseEntity
    {
      
        public Guid OutlineId { get; set; } // FK
        public string Content { get; set; } = null!;

        // Navigation Property
        public Outline Outline { get; set; } = null!;

        // 1 ContentBlock → N Review
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
