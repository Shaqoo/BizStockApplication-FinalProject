using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Products.GetRecentlyAddedProducts
{

    public class GetRecentlyAddedProductsHandler(IMemoryCacheService distributedCacheService,
        IProductRepository productRepository) : IRequestHandler<GetRecentlyAddedProductsQuery, Result<PaginatedList<ProductDto>>>
    {
        public async Task<Result<PaginatedList<ProductDto>>> Handle(GetRecentlyAddedProductsQuery request, CancellationToken cancellationToken)
        {
             string cacheKey = $"GetRecentlyAddedProducts_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

             var products = await distributedCacheService.GetOrAddAsync(cacheKey,
                 async () =>
                 {
                     var paginatedProducts = await productRepository.GetRecentlyAddedProductsAsync(request.PageRequest);
                     return paginatedProducts;
                 }, TimeSpan.FromMinutes(5));

            return Result<PaginatedList<ProductDto>>.Success(new PaginatedList<ProductDto>(products.Items
                .Select(a => a.ToDto()).ToList(),
                products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
