using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Products.GetRelatedProducts
{
    public class GetRelatedProductsHandler(IProductRepository productRepository,
        IMemoryCacheService memoryCacheService)
        : IRequestHandler<GetRelatedProductQuery, Result<IEnumerable<ProductDto>>>
    {
        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetRelatedProductQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetRelatedProductQuery:{request.ProductId}";

            var result = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var product = await productRepository.GetByIdAsync(request.ProductId);
                    if (product == null)
                    {
                        return null;
                    }
                    var products = await productRepository.GetRelatedProducts(product);
                    return products;
                });

            return result == null ? Result<IEnumerable<ProductDto>>.Failure("Product Not Found") :
                Result<IEnumerable<ProductDto>>.Success(result);
        }
    }
}
