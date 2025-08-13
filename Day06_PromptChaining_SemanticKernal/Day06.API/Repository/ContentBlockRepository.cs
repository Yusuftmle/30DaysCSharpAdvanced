using Day06.API.Context.cs;
using Day06.API.Models;
using Day06.API.Repository.Interfaces;
using static Day06.API.Repository.ContentBlockRepository;

namespace Day06.API.Repository
{
    public class ContentBlockRepository : GenericRepository<ContentBlock>, IContentBlockRepository
    {
       
            public ContentBlockRepository(BlogPostContext dbContext) : base(dbContext, dbContext.Set<ContentBlock>())
            {

            }

        
    }
}
