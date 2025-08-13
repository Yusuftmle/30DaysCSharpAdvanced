using Day06.API.Models;
using Day06.API.Repository.Interfaces;
using MediatR;

namespace Day06.API.CommandQueries.command
{
    public class CreateOutlineCommandHandler : IRequestHandler<CreateOutlineCommand, Guid>
    {
        private readonly IOutlineRepository _repository;
        public  async Task<Guid> Handle(CreateOutlineCommand request, CancellationToken cancellationToken)
        {
            var entity = new Outline
            {
                Id = Guid.NewGuid(),
                BlogPostId = request.BlogPostId,
                Title = request.Title,
                Order = request.Order
            };

            await _repository.AddAsync(entity);
            return entity.Id;
        }
    }
}
