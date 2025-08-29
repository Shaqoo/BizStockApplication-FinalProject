using Application.Dto;
using Application.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Specifications.GetSpecificationById
{
    public class GetSpecificationByIdQueryHandler
    : IRequestHandler<GetSpecificationByIdQuery, Result<SpecificationDto>>
    {
        private readonly ISpecificationRepository _repository;
        private readonly ILogger<GetSpecificationByIdQueryHandler> _logger;

        public GetSpecificationByIdQueryHandler(
            ISpecificationRepository repository,
            ILogger<GetSpecificationByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<SpecificationDto>> Handle(GetSpecificationByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = await _repository.GetByIdAsync(request.Id);
            if (spec is null)
            {
                _logger.LogWarning("Specification {Id} not found", request.Id);
                return Result<SpecificationDto>.Failure("Specification not found");
            }

            return Result<SpecificationDto>.Success(new SpecificationDto
            {
                Id = spec.Id,
                Name = spec.Name,
                Description = spec.Description ?? string.Empty
            });
        }
    }

}
