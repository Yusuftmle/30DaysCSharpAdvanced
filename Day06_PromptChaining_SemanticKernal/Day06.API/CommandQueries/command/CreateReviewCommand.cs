using MediatR;

namespace Day06.API.CommandQueries.command
{
    public class CreateReviewCommand:IRequest<Guid>
    {
        public Guid ContentBlockId { get; set; }
        public string Notes { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
