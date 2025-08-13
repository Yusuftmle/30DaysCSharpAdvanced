using MediatR;

namespace Day06.API.CommandQueries.command
{
    public class BlogPostCommand:IRequest<Guid>
    {
        public string title { get; set; }
        public string content { get; set; }
    }
}
