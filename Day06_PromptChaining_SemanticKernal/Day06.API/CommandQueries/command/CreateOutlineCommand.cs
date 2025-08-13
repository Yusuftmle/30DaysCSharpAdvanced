using MediatR;

namespace Day06.API.CommandQueries.command
{
    public class CreateOutlineCommand:IRequest<Guid>
    {
        public Guid BlogPostId { get; set; }
        public string Title { get; set; } = null!;
        public int Order { get; set; }
    }
}
