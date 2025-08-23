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

namespace Application.Queries.Products.SearchProducts
{
    public class SearchProductsHandler(IProductRepository productRepository,
        IMemoryCacheService distributedCache) : IRequestHandler<SearchProductsQuery, Result<PaginatedList<ProductDto>>>
    {
        public async Task<Result<PaginatedList<ProductDto>>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"SearchProducts-{request.keyword}-{request.PageRequest.Page}-{request.PageRequest.PageSize}";

            var products = await distributedCache.GetOrAddAsync(cacheKey, async () =>
            {
                var products = await productRepository.SearchProductsAsync(request.keyword, request.PageRequest);
                return products;
            },TimeSpan.FromMinutes(5));

            return Result<PaginatedList<ProductDto>>.Success(new PaginatedList<ProductDto>(products.Items
                .Select(a => a.ToDto()).ToList(), products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
