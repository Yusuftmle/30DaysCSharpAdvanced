namespace Day06.API.Models
{
    public class Review: BaseEntity
    {
     
        public Guid ContentBlockId { get; set; } // FK
        public string Notes { get; set; } = null!;
        public string Status { get; set; } = null!; // Örn: "Pending", "Approved", "NeedsChanges"

        // Navigation Property
        public ContentBlock ContentBlock { get; set; } = null!;
    }
}
