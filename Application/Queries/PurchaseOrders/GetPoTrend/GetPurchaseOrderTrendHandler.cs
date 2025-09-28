using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.PurchaseOrders.GetPoTrend
{
    public class GetPurchaseOrderTrendHandler(IPurchaseOrderRepository purchaseOrderRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetPurchaseOrderTrendQuery, Result<PoTrendDto>>
    {
        public async Task<Result<PoTrendDto>> Handle(GetPurchaseOrderTrendQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "PoTrendDto";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var trend = await purchaseOrderRepository.GetMonthlyPurchaseOrderTrendsAsync();
                return Result<PoTrendDto>.Success(trend);
            },TimeSpan.FromMinutes(5));

            return cachedResult ?? new Result<PoTrendDto>();
        }
    }
}
