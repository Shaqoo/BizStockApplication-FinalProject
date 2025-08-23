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

namespace Application.Queries.Products.GetProductsByStatus
{
    public class GetProductsByStatusHandler(IMemoryCacheService distributedCacheService,
        IProductRepository productRepository) : IRequestHandler<GetProductsByStatusQuery, Result<PaginatedList<ProductDto>>>
    {
        public async Task<Result<PaginatedList<ProductDto>>> Handle(GetProductsByStatusQuery request, CancellationToken cancellationToken)
        {
             string cacheKey = $"GetProductsByStatus_{request.ProductStatus}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

             var products = await distributedCacheService.GetOrAddAsync(cacheKey,
                 async () =>
                 {
                     var paginatedProducts = await productRepository.GetProductsByStatus(request.PageRequest, request.ProductStatus);
                     return paginatedProducts;
                 }, TimeSpan.FromMinutes(5));

            return Result<PaginatedList<ProductDto>>.Success(new PaginatedList<ProductDto>(products.Items
                .Select(a => a.ToDto()).ToList(),
                products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
