using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Nest;
using System.Collections;

namespace Application.Queries.Products.GetRelatedProductsByBrand
{
    public class GetRelatedProductsByBrandHandler(IProductRepository productRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetRelatedProductsByBrandQuery, Result<IEnumerable<ProductDto>>>
    {
        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetRelatedProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetRelatedProductsByBrandQuery:{request.ProductId}";

            var result = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var product = await productRepository.GetByIdAsync(request.ProductId);
                    if (product == null)
                    {
                        return null;
                    }
                    var products = await productRepository.GetRelatedProductsByBrand(product);
                    return products;
                });

            return result == null ? Result<IEnumerable<ProductDto>>.Failure("Product Not Found") :
                Result<IEnumerable<ProductDto>>.Success(result);
        }
    }

}
