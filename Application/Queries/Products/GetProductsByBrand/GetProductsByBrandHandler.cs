using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Products.GetProductsByBrand
{
    public class GetProductsByBrandHandler(IProductRepository productRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<GetProductsByBrandQuery, Result<PaginatedList<ProductDto>>>
    {
        public async Task<Result<PaginatedList<ProductDto>>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"ProductsByBrand-{request.brandId}-{request.PageRequest.Page}-{request.PageRequest.PageSize}";

            var products = await distributedCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var products = await productRepository.GetProductsByBrandId(request.brandId, request.PageRequest);
                return products;
            }, TimeSpan.FromMinutes(5));

            return Result<PaginatedList<ProductDto>>.Success(new PaginatedList<ProductDto>(products.Items
                .Select(a => a.ToDto()).ToList(), products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
