using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Queries.Products.GetProductStats;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.PurchaseOrders.GetProductStats
{
    public class GetProductStatsHandler(
        IProductRepository productRepository,
        IMemoryCacheService memoryCacheService,
        ILogger<GetProductStatsHandler> logger)
        : IRequestHandler<GetProductStatsQuery, Result<ProductStatsDto>>
    {
        public async Task<Result<ProductStatsDto>> Handle(GetProductStatsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = "product_stats";

                var stats = await memoryCacheService.GetOrAddAsync(
                    cacheKey,
                    async () => await productRepository.GetProductStatsAsync(),
                    TimeSpan.FromMinutes(5)
                );

                return Result<ProductStatsDto>.Success(stats);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching product stats");
                return Result<ProductStatsDto>.Failure("An error occurred while fetching product stats.");
            }
        }
    }
}
