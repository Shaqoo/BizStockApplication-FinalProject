using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Specifications.UpdateSpecification
{
    public class UpdateSpecificationCommandHandler : IRequestHandler<UpdateSpecificationCommand, Result<string>>
    {
        private readonly ISpecificationRepository _specificationRepository;
        private readonly ILogger<UpdateSpecificationCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSpecificationCommandHandler(
            ISpecificationRepository specificationRepository,
            ILogger<UpdateSpecificationCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _specificationRepository = specificationRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(UpdateSpecificationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var request = command.Request;

                var specification = await _specificationRepository.GetByIdAsync(request.SpecificationId);
                if (specification is null)
                {
                    _logger.LogWarning("Specification with Id {Id} not found.", request.SpecificationId);
                    return Result<string>.Failure($"Specification with Id {request.SpecificationId} not found.");
                }

                specification.UpdateName(request.Name);
                specification.UpdateDescription(request.Description);

                await _specificationRepository.Update(specification);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Specification with Id {Id} updated successfully.", request.SpecificationId);

                return Result<string>.Success("Specification updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating specification with Id {Id}", command.Request.SpecificationId);
                return Result<string>.Failure("An error occurred while updating the specification.");
            }
        }
    }

}
