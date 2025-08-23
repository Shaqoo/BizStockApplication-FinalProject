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

namespace Application.Queries.Products.GetProductsOrderdByPrice
{
    public class GetProductsOrderedByPriceHandler(IProductRepository productRepository,
        IMemoryCacheService distributedCache) : IRequestHandler<GetProductsOrderedByPriceQuery, Result<PaginatedList<ProductDto>>>
    {
        public async Task<Result<PaginatedList<ProductDto>>> Handle(GetProductsOrderedByPriceQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"ProductsOrderedByPrice-{request.PageRequest.Page}-{request.PageRequest.PageSize}";

            var products = await distributedCache.GetOrAddAsync(cacheKey, async () =>
            {
                var products = await productRepository.GetProductsOrderedByPrice(request.PageRequest,request.ascending);
                return products;
            }, TimeSpan.FromMinutes(5));

            return Result<PaginatedList<ProductDto>>.Success(new PaginatedList<ProductDto>(products.Items
                .Select(a => a.ToDto()).ToList(), products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
