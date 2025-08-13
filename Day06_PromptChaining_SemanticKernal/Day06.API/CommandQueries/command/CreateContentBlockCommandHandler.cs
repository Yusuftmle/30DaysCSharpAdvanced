using Day06.API.Models;
using Day06.API.Repository.Interfaces;
using MediatR;

namespace Day06.API.CommandQueries.command
{
    public class CreateContentBlockCommandHandler : IRequestHandler<CreateContentBlockCommand, Guid>
    {
        private readonly IContentBlockRepository _repository;

        public CreateContentBlockCommandHandler(IContentBlockRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateContentBlockCommand request, CancellationToken cancellationToken)
        {
            var entity = new ContentBlock
            {
                Id = Guid.NewGuid(),
                OutlineId = request.OutlineId,
                Content = request.Content
            };

            await _repository.AddAsync(entity); // AddAsync metodun repository’de olmalı
            return entity.Id;
        }
    }
}
