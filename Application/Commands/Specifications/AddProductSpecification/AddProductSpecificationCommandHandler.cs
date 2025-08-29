using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Specifications.AddProductSpecification
{
    public class AddProductSpecificationCommandHandler
        : IRequestHandler<AddProductSpecificationCommand, Result<Guid>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ISpecificationRepository _specificationRepository;
        private readonly IProductSpecificationRepository _productSpecificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddProductSpecificationCommandHandler> _logger;

        public AddProductSpecificationCommandHandler(
            IProductRepository productRepository,
            ISpecificationRepository specificationRepository,
            IProductSpecificationRepository productSpecificationRepository,
            IUnitOfWork unitOfWork,
            ILogger<AddProductSpecificationCommandHandler> logger)
        {
            _productRepository = productRepository;
            _specificationRepository = specificationRepository;
            _productSpecificationRepository = productSpecificationRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(AddProductSpecificationCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", request.ProductId);
                return Result<Guid>.Failure($"Product with ID {request.ProductId} not found");
            }

            var specification = await _specificationRepository.GetByIdAsync(request.SpecificationId);
            if (specification == null)
            {
                _logger.LogWarning("Specification with ID {SpecificationId} not found", request.SpecificationId);
                return Result<Guid>.Failure($"Specification with ID {request.SpecificationId} not found");
            }

            var existing = await _productSpecificationRepository
                .GetByProductAndSpecificationAsync(request.ProductId, request.SpecificationId);

            if (existing != null)
            {
                _logger.LogWarning("ProductSpecification already exists for Product {ProductId} and Spec {SpecificationId}", request.ProductId, request.SpecificationId);
                return Result<Guid>.Failure("This specification is already assigned to the product.");
            }

            var productSpecification = new ProductSpecification(request.ProductId, request.SpecificationId, request.Value);

            await _productSpecificationRepository.AddAsync(productSpecification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added ProductSpecification {Id} for Product {ProductId} and Specification {SpecificationId}",
                productSpecification.Id, request.ProductId, request.SpecificationId);

            return Result<Guid>.Success(productSpecification.Id);
        }
    }

}
