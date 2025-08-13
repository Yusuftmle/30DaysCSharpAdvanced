using Day06.API.Models;
using Day06.API.Repository.Interfaces;
using MediatR;

namespace Day06.API.CommandQueries.command
{
    public class CreateReviewCommandHandler:IRequestHandler<CreateReviewCommand,Guid>
    {

        private readonly IReviewRepository _repository;

        public CreateReviewCommandHandler(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var entity = new Review
            {
                Id = Guid.NewGuid(),
                ContentBlockId = request.ContentBlockId,
                Notes = request.Notes,
                Status = request.Status
            };

            await _repository.AddAsync(entity);
            return entity.Id;
        }
    }
}
