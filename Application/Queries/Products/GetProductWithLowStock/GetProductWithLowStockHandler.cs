using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Products.GetProductWithLowStock
{
    public class GetProductWithLowStockHandler(IProductRepository productRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<GetProductWithLowStockQuery, Result<PaginatedList<ProductDto>>>
    {
        public async Task<Result<PaginatedList<ProductDto>>> Handle(GetProductWithLowStockQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetProductWithLowStock_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var products = await distributedCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var paginatedProducts = await productRepository.GetProductsWithLowStockAsync(request.PageRequest);
                    return paginatedProducts;
                }, TimeSpan.FromMinutes(5));

            return Result<PaginatedList<ProductDto>>.Success(new PaginatedList<ProductDto>(products.Items
               .Select(a => a.ToDto()).ToList(),
               products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
