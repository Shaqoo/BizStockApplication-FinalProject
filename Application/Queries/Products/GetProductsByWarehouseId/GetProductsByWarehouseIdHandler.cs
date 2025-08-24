using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Products.GetProductsByWarehouseId
{
    public class GetProductsByWarehouseIdHandler(IWarehouseItemRepository warehouseItemRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<GetProductsByWarehouseIdQuery, Result<PaginatedList<WarehouseProductDto>>>
    {
        public async Task<Result<PaginatedList<WarehouseProductDto>>> Handle(GetProductsByWarehouseIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"ProductsByWarehouse-{request.WarehouseId}-{request.PageRequest.Page}-{request.PageRequest.PageSize}";

            var products = await distributedCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var products = await warehouseItemRepository.GetItemsByWarehouseIdAsync(request.WarehouseId, request.PageRequest);
                return products;
            },TimeSpan.FromMinutes(5));

            return Result<PaginatedList<WarehouseProductDto>>.Success(new PaginatedList<WarehouseProductDto>(products.Items
                .Select(a => a.WarehouseProductDto()).ToList(), products.TotalCount, products.PageNumber, products.PageSize));
        }
    }
}
