using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.StockMovements.GetStats
{
    public class GetStockMovementsStatsHandler(
     IStockMovementRepository stockMovementRepository,
     IMemoryCacheService memoryCacheService,
     ILogger<GetStockMovementsStatsHandler> logger
 ) : IRequestHandler<GetStockMovementsStatsQuery, Result<StockMovementStatsDto>>
    {
        public async Task<Result<StockMovementStatsDto>> Handle(GetStockMovementsStatsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "StockMovementStats";

            var cachedStats = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                logger.LogInformation("Cache miss for {CacheKey}, fetching from DB...", cacheKey);
                return await stockMovementRepository.GetStockMovementStatsAsync();
            }, TimeSpan.FromMinutes(10));

            logger.LogInformation("Returning stock movement stats for {CacheKey}", cacheKey);

            return Result<StockMovementStatsDto>.Success(cachedStats);
        }
    }

}
