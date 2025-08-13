using Day06.API.CommandQueries.command;
using Day06.API.Models;
using Day06.API.Repository.Interfaces;
using MediatR;

namespace Day06.API.CommandQueries.Command
{
    public class BlogPostCommandHandler : IRequestHandler<BlogPostCommand, Guid>
    {
        private readonly IBlogPostEntityRepository _blogPostEntityRepository;

        public BlogPostCommandHandler(IBlogPostEntityRepository blogPostEntityRepository)
        {
            _blogPostEntityRepository = blogPostEntityRepository;
        }

        public async Task<Guid> Handle(BlogPostCommand request, CancellationToken cancellationToken)
        {
            // Entity oluştur
            var blog = new BlogPostEntity
            {
                Id = Guid.NewGuid(),
                title = request.title,
                content = request.content,
                createdDate = DateTime.UtcNow,
                updatedDate = DateTime.UtcNow
            };

            // Kaydet
         await _blogPostEntityRepository.AddAsync(blog);

            // Id dön
            return blog.Id;
        }
    }
}
