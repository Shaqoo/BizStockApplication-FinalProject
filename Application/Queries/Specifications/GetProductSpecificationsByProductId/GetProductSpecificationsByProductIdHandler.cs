using Application.Dto;
using Application.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Specifications.GetProductSpecificationsByProductId
{
    public class GetProductSpecificationsByProductIdHandler
    : IRequestHandler<GetProductSpecificationsByProductIdQuery, Result<ProductSpecificationListDto>>
    {
        private readonly IProductSpecificationRepository _productSpecificationRepo;
        private readonly ISpecificationRepository _specificationRepo;
        private readonly ILogger<GetProductSpecificationsByProductIdHandler> _logger;

        public GetProductSpecificationsByProductIdHandler(
            IProductSpecificationRepository productSpecificationRepo,
            ISpecificationRepository specificationRepo,
            ILogger<GetProductSpecificationsByProductIdHandler> logger)
        {
            _productSpecificationRepo = productSpecificationRepo;
            _specificationRepo = specificationRepo;
            _logger = logger;
        }

        public async Task<Result<ProductSpecificationListDto>> Handle(
            GetProductSpecificationsByProductIdQuery request,
            CancellationToken cancellationToken)
        {
            var specs = await _productSpecificationRepo.GetByProductIdAsync(request.ProductId);
            if (!specs.Any())
            {
                _logger.LogWarning("No product specifications found for ProductId {ProductId}", request.ProductId);
                return Result<ProductSpecificationListDto>.Failure("No specifications found");
            }

            var dto = new ProductSpecificationListDto
            {
                ProductId = request.ProductId,
                Specifications = specs.Select(s => new ProductSpecificationDto
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    SpecificationId = s.SpecificationId,
                    SpecificationName = s.Specification.Name ?? string.Empty,
                    Value = s.Value
                }).ToList()
            };

            return Result<ProductSpecificationListDto>.Success(dto);
        }
    }

}
