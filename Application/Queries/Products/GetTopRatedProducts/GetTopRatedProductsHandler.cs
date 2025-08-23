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

namespace Application.Queries.Products.GetTopRatedProducts
{
    public class GetTopRatedProductsHandler(IProductRepository productRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<GetTopRatedProductsQuery, Result<PaginatedList<ProductDto>>>
    {
        public async Task<Result<PaginatedList<ProductDto>>> Handle(GetTopRatedProductsQuery request, CancellationToken cancellationToken)
        {
             string cacheKey = $"GetTopRatedProducts_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

             var products = await distributedCacheService.GetOrAddAsync(cacheKey,
                 async () =>
                 {
                        var paginatedProducts = await productRepository.GetTopRatedProductsAsync(request.PageRequest);
                     return paginatedProducts;
                    }, TimeSpan.FromMinutes(5));

            return Result<PaginatedList<ProductDto>>.Success(new PaginatedList<ProductDto>(products.Items
                .Select(a => a.ToDto()).ToList(), products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
