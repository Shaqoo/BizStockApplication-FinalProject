using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Products.GetById
{
    public class GetProductByIdHandler(IProductRepository productRepository,
        IMemoryCacheService distributedCache) : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Product-{request.Id}";

            var cachedProduct = await distributedCache.GetOrAddAsync(cacheKey, async () =>
            {
                var product = await productRepository.GetByIdAsync(request.Id);
                return product;
            }, TimeSpan.FromMinutes(5));

            if (cachedProduct is null)
            {
                return Result<ProductDto>.Failure("Product not found");
            }
            return Result<ProductDto>.Success(cachedProduct.ToDto());
        }
    }
}
