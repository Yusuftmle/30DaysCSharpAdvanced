using Day06.API.Context.cs;
using Day06.API.Models;
using Day06.API.Repository.Interfaces;

namespace Day06.API.Repository
{
    public class ReviewRepository: GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(BlogPostContext dbContext) : base(dbContext, dbContext.Set<Review>())
        {

        }
    }
}
