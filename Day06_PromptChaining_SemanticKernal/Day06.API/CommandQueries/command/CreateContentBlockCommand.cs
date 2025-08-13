using MediatR;

namespace Day06.API.CommandQueries.command
{
    public class CreateContentBlockCommand:IRequest<Guid>
    {
        public Guid OutlineId { get; set; }
        public string Content { get; set; } = null!;
    }
}
