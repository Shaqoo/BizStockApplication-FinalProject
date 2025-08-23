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

namespace Application.Queries.Products.GetProducts
{
    public class GetProductsHandler(IProductRepository productRepository,
        IMemoryCacheService distributedCache) : IRequestHandler<GetProductsQuery, Result<PaginatedList<ProductDto>>>
    {
        public async Task<Result<PaginatedList<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"Products-{request.PageRequest.Page}-{request.PageRequest.PageSize}";

            var products = await distributedCache.GetOrAddAsync(cacheKey, async () =>
            {
                var products = await productRepository.GetAllAsync(request.PageRequest);
                return products;
            });

            return Result<PaginatedList<ProductDto>>.Success(new PaginatedList<ProductDto>(products.Items
                .Select(a => a.ToDto()).ToList(), products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
