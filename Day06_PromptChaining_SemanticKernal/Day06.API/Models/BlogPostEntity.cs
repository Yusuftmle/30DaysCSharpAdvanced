namespace Day06.API.Models
{
    public class BlogPostEntity:BaseEntity
    {
      
        public string title { get; set; }
        public string content { get; set; }

        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        // Navigation Property (1 BlogPost → N Outline)
        public ICollection<Outline> Outlines { get; set; } = new List<Outline>();
    }
}
