using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Specifications.DeleteSpecification
{
    public class DeleteSpecificationCommandHandler
        : IRequestHandler<DeleteSpecificationCommand, Result<string>>
    {
        private readonly ISpecificationRepository _specificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteSpecificationCommandHandler> _logger;

        public DeleteSpecificationCommandHandler(
            ISpecificationRepository specificationRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteSpecificationCommandHandler> logger)
        {
            _specificationRepository = specificationRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(
            DeleteSpecificationCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var spec = await _specificationRepository.GetByIdAsync(command.Id);
                if (spec is null)
                {
                    _logger.LogWarning("Delete failed: Specification with Id {Id} not found.", command.Id);
                    return Result<string>.Failure("Specification not found.");
                }

                if(spec.ProductSpecifications.Any())
                {
                    _logger.LogWarning("Cannot Delete Specification Related To Products");
                    return Result<string>.Failure("Cannot Delete Specification Related To Products");
                }

                await _specificationRepository.Remove(spec);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Specification with Id {Id} deleted successfully.", command.Id);
                return Result<string>.Success("Specification deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting specification with Id {Id}", command.Id);
                return Result<string>.Failure("An error occurred while deleting specification.");
            }
        }
    }

}
