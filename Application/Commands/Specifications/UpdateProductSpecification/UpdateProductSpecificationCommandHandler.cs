using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Specifications.UpdateProductSpecification
{
    public class UpdateProductSpecificationCommandHandler
        : IRequestHandler<UpdateProductSpecificationCommand, Result<string>>
    {
        private readonly IProductSpecificationRepository _productSpecificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateProductSpecificationCommandHandler> _logger;

        public UpdateProductSpecificationCommandHandler(
            IProductSpecificationRepository productSpecificationRepository,
            IProductRepository productRepository,
            ISpecificationRepository specificationRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateProductSpecificationCommandHandler> logger)
        {
            _productSpecificationRepository = productSpecificationRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(
            UpdateProductSpecificationCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var request = command.Request;

                var productSpec = await _productSpecificationRepository.GetByIdAsync(request.ProductSpecificationId);
                if (productSpec is null)
                {
                    _logger.LogWarning("Update failed: ProductSpecification with Id {Id} not found.", request.ProductSpecificationId);
                    return Result<string>.Failure("Product specification not found.");
                }

                productSpec.UpdateValue(request.Value);

                await _productSpecificationRepository.Update(productSpec);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("ProductSpecification with Id {Id} updated successfully.", request.ProductSpecificationId);
                return Result<string>.Success("Product specification updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating ProductSpecification with Id {Id}", command.Request.ProductSpecificationId);
                return Result<string>.Failure("An error occurred while updating product specification.");
            }
        }
    }

}
