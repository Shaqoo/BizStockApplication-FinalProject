using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Specifications.CreateSpecification
{
    public class CreateSpecificationCommandHandler
        : IRequestHandler<CreateSpecificationCommand, Result<Guid>>
    {
        private readonly ISpecificationRepository _specificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateSpecificationCommandHandler> _logger;

        public CreateSpecificationCommandHandler(
            ISpecificationRepository specificationRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateSpecificationCommandHandler> logger)
        {
            _specificationRepository = specificationRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(
            CreateSpecificationCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var request = command.Request;

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    _logger.LogWarning("Specification creation failed: Name is empty.");
                    return Result<Guid>.Failure("Specification name cannot be empty.");
                }

                var existing = await _specificationRepository.GetByNameAsync(request.Name);
                if (existing is not null)
                {
                    _logger.LogWarning("Specification with name {Name} already exists.", request.Name);
                    return Result<Guid>.Failure($"Specification '{request.Name}' already exists.");
                }

                await _unitOfWork.BeginTransactionAsync();

                var specification = new Specification(request.Name, request.Description);

                await _specificationRepository.AddAsync(specification);
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Specification {Name} created with Id {Id}", request.Name, specification.Id);

                return Result<Guid>.Success(specification.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error while creating specification.");
                return Result<Guid>.Failure("An error occurred while creating specification.");
            }
        }
    }

}
