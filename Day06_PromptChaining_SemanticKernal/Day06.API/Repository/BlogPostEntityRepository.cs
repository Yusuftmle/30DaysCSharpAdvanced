using Day06.API.Context.cs;
using Day06.API.Models;
using Day06.API.Repository.Interfaces;

namespace Day06.API.Repository
{
    public class BlogPostEntityRepository : GenericRepository<BlogPostEntity>, IBlogPostEntityRepository
    {
        public BlogPostEntityRepository(BlogPostContext dbContext) : base(dbContext, dbContext.Set<BlogPostEntity>())
        {

        }

    }
}
