using Day06.API.Context.cs;
using Day06.API.Models;
using Day06.API.Repository.Interfaces;

namespace Day06.API.Repository
{
    public class OutlineRepository: GenericRepository<Outline>, IOutlineRepository
    {
        public OutlineRepository(BlogPostContext dbContext) : base(dbContext, dbContext.Set<Outline>())
        {

        }
    }
}
