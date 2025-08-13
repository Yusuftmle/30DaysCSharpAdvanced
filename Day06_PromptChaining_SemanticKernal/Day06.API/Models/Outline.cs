namespace Day06.API.Models
{
    public class Outline: BaseEntity
    {
       
        public Guid BlogPostId { get; set; } // FK
        public string Title { get; set; } = null!;
        public int Order { get; set; }

        // Navigation Property
        public BlogPostEntity BlogPost { get; set; } = null!;

        // 1 Outline → N ContentBlock
        public ICollection<ContentBlock> ContentBlocks { get; set; } = new List<ContentBlock>();
    }
}
