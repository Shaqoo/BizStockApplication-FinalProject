using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.StockMovements.GetStockMovementTrend
{
    public class GetStockMovementTrendQueryHandler
        : IRequestHandler<GetStockMovementTrendQuery, Result<List<StockMovementTrendDto>>>
    {
        private readonly IStockMovementRepository _repository;
        private readonly IMemoryCacheService _memoryCacheService;

        public GetStockMovementTrendQueryHandler(IStockMovementRepository repository,IMemoryCacheService memoryCacheService)
        {
            _repository = repository;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<Result<List<StockMovementTrendDto>>> Handle(
            GetStockMovementTrendQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"StockMovementTrend_{request.Range}";
                var cachedTrend = await _memoryCacheService.GetAsync<List<StockMovementTrendDto>>(cacheKey);
                if (cachedTrend != null)
                {
                    return Result<List<StockMovementTrendDto>>.Success(cachedTrend);
                }

                var trend = await _repository.GetStockMovementTrendAsync(request.Range);
                await _memoryCacheService.SetAsync(cacheKey, trend, TimeSpan.FromMinutes(10));


                return Result<List<StockMovementTrendDto>>.Success(trend);
            }
            catch (Exception ex)
            {
                return Result<List<StockMovementTrendDto>>.Failure(
                    $"Failed to fetch stock movement trend: {ex.Message}"
                );
            }
        }
    }
}
