using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Specifications.RemoveProductSpecification
{
    public class RemoveProductSpecificationCommandHandler
        : IRequestHandler<RemoveProductSpecificationCommand, Result<string>>
    {
        private readonly IProductSpecificationRepository _productSpecificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveProductSpecificationCommandHandler> _logger;

        public RemoveProductSpecificationCommandHandler(
            IProductSpecificationRepository productSpecificationRepository,
            IUnitOfWork unitOfWork,
            ILogger<RemoveProductSpecificationCommandHandler> logger)
        {
            _productSpecificationRepository = productSpecificationRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(
            RemoveProductSpecificationCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var productSpec = await _productSpecificationRepository.GetByIdAsync(command.ProductSpecificationId);
                if (productSpec is null)
                {
                    _logger.LogWarning("Remove failed: ProductSpecification with Id {Id} not found.", command.ProductSpecificationId);
                    return Result<string>.Failure("Product specification not found.");
                }

                await _productSpecificationRepository.Remove(productSpec);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("ProductSpecification with Id {Id} removed successfully.", command.ProductSpecificationId);
                return Result<string>.Success("Product specification removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing ProductSpecification with Id {Id}", command.ProductSpecificationId);
                return Result<string>.Failure("An error occurred while removing product specification.");
            }
        }
    }

}
