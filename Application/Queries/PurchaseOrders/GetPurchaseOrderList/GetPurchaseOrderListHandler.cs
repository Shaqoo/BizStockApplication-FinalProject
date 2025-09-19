using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrderList
{
    public class GetPurchaseOrderListHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IMemoryCacheService memoryCacheService,
        ILogger<GetPurchaseOrderListHandler> logger
    ) : IRequestHandler<GetPurchaseOrderListQuery, PaginatedList<PurchaseOrderListDto>>
    {
        public async Task<PaginatedList<PurchaseOrderListDto>> Handle(GetPurchaseOrderListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"PurchaseOrders:Page{request.PageRequest.Page}:Size{request.PageRequest.PageSize}";

                var result = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
                {
                    logger.LogInformation("Cache miss for {CacheKey}. Fetching purchase orders from database...", cacheKey);

                    var paginatedOrders = await purchaseOrderRepository.GetAllWithDtoAsync(request.PageRequest);

                    return paginatedOrders;
                },TimeSpan.FromMinutes(5));

                logger.LogInformation("Successfully retrieved {Count} purchase orders (Page {Page}, Size {Size})",
                    result.Items.Count, request.PageRequest.Page, request.PageRequest.PageSize);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving purchase order list (Page {Page}, Size {Size})",
                    request.PageRequest.Page, request.PageRequest.PageSize);

                return new PaginatedList<PurchaseOrderListDto>(
                    new List<PurchaseOrderListDto>(), 0, request.PageRequest.Page, request.PageRequest.PageSize
                );
            }
        }
    }
}
