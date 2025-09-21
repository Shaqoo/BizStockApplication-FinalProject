using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrdersByDateRange
{
    public class GetPurchaseOrdersByDateRangeHandler(IPurchaseOrderRepository purchaseOrderRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetPurchaseOrdersByDateRangeQuery, Result<PaginatedList<PurchaseOrderListDto>>>
    {
        public async Task<Result<PaginatedList<PurchaseOrderListDto>>> Handle(GetPurchaseOrdersByDateRangeQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetPurchaseOrdersByDateRange-{request.StartDate}-{request.EndDate}-{request.PageRequest.Page}-{request.PageRequest.PageSize}";
            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var purchaseOrders = await purchaseOrderRepository.GetPurchaseOrdersByDateRangeAsync(request.StartDate, request.EndDate, request.PageRequest);
               
                return Result<PaginatedList<PurchaseOrderListDto>>.Success(purchaseOrders);
            }, TimeSpan.FromMinutes(10));

            return cachedResult ?? new Result<PaginatedList<PurchaseOrderListDto>>();
        }
    }
}
