using Application.Dto;
using Application.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Specifications.GetAllSpecifications
{
    public class GetAllSpecificationsQueryHandler
        : IRequestHandler<GetAllSpecificationsQuery, Result<List<SpecificationDto>>>
    {
        private readonly ISpecificationRepository _repository;
        private readonly ILogger<GetAllSpecificationsQueryHandler> _logger;

        public GetAllSpecificationsQueryHandler(
            ISpecificationRepository repository,
            ILogger<GetAllSpecificationsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<List<SpecificationDto>>> Handle(GetAllSpecificationsQuery request, CancellationToken cancellationToken)
        {
            var specs = await _repository.GetAllAsync();

            var dtos = specs.Select(s => new SpecificationDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description ?? string.Empty
            }).ToList();

            _logger.LogInformation("Retrieved {Count} specifications", dtos.Count);

            return Result<List<SpecificationDto>>.Success(dtos);
        }
    }

}
