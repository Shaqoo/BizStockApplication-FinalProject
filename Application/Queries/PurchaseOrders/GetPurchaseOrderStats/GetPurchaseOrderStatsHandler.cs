using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrderStats
{
    public class GetPurchaseOrderStatsHandler : IRequestHandler<GetPurchaseOrderStatsQuery, Result<PurchaseOrderStatsDto>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ILogger<GetPurchaseOrderStatsHandler> _logger;
        private readonly IMemoryCacheService _memoryCacheService;

        public GetPurchaseOrderStatsHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            ILogger<GetPurchaseOrderStatsHandler> logger,
            IMemoryCacheService memoryCacheService)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<Result<PurchaseOrderStatsDto>> Handle(GetPurchaseOrderStatsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = "purchase-order-stats";

                var stats = await _memoryCacheService.GetOrAddAsync(cacheKey, async () =>
                {
                    _logger.LogInformation("Fetching purchase order stats from database...");
                    return await _purchaseOrderRepository.GetPurchaseOrderStatsAsync();
                },TimeSpan.FromMinutes(5));

                if (stats == null)
                {
                    _logger.LogWarning("No purchase order stats found");
                    return Result<PurchaseOrderStatsDto>.Failure("No stats available");
                }

                _logger.LogInformation("Successfully retrieved purchase order stats");
                return Result<PurchaseOrderStatsDto>.Success(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving purchase order stats");
                return Result<PurchaseOrderStatsDto>.Failure("An error occurred while retrieving stats");
            }
        }
    }
}
