using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Products.GetProductsByPriceRange
{
    public class GetProductsByPriceRangeHandler(IProductRepository productRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<GetProductsByPriceRangeQuery, Result<PaginatedList<ProductDto>>>
    {
        public async Task<Result<PaginatedList<ProductDto>>> Handle(GetProductsByPriceRangeQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"ProductsByPriceRange-{request.minPrice}-{request.maxPrice}-{request.PageRequest.Page}-{request.PageRequest.PageSize}";

            var products = await distributedCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var products = await productRepository.GetProductsByPriceRangeAsync(request.minPrice, request.maxPrice, request.PageRequest);
                return products;
            }, TimeSpan.FromMinutes(5));

            return Result<PaginatedList<ProductDto>>.Success(new PaginatedList<ProductDto>(products.Items
                .Select(a => a.ToDto()).ToList(), products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
