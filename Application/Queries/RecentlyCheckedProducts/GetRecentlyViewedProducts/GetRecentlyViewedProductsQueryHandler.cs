using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.RecentlyCheckedProducts.GetRecentlyViewedProducts
{
    public class GetRecentlyViewedProductsQueryHandler
        : IRequestHandler<GetRecentlyViewedProductsQuery, Result<RecentlyViewedProductsDto>>
    {
        private readonly IRecentlyViewedProductRepository _repository;
        private readonly ILogger<GetRecentlyViewedProductsQueryHandler> _logger;
        private readonly IMemoryCacheService _memoryCacheService;

        public GetRecentlyViewedProductsQueryHandler(
            IRecentlyViewedProductRepository repository,
            IMemoryCacheService memoryCacheService,
            ILogger<GetRecentlyViewedProductsQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<Result<RecentlyViewedProductsDto>> Handle(
            GetRecentlyViewedProductsQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                
                var cacheKey = query.UserId.HasValue
                    ? $"recently_viewed_user_{query.UserId}"
                    : $"recently_viewed_session_{query.SessionId}";

                
                var cached = await _memoryCacheService.GetAsync<RecentlyViewedProductsDto>(cacheKey);
                if (cached != null)
                {
                    _logger.LogInformation("Returning cached recently viewed products for {Id}",
                        query.UserId?.ToString() ?? query.SessionId);

                    return Result<RecentlyViewedProductsDto>.Success(cached);
                }

                RecentlyViewedProducts? entity = null;

                if (query.UserId.HasValue)
                    entity = await _repository.GetByUserIdAsync(query.UserId.Value);
                else if (!string.IsNullOrEmpty(query.SessionId))
                    entity = await _repository.GetBySessionIdAsync(query.SessionId);

                if (entity == null)
                    return Result<RecentlyViewedProductsDto>.Failure("No recently viewed products found.");

                var dto = new RecentlyViewedProductsDto
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    SessionId = entity.SessionId,
                    Items = entity.Items
                        .Select(i => new RecentlyViewedProductDto
                        {
                            ProductId = i.ProductId,
                            ViewedAt = i.DateReviewed
                        })
                        .ToList()
                };

               
                await _memoryCacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

                _logger.LogInformation("Fetched {Count} recently viewed products for {Id}",
                    dto.Items.Count, entity.UserId ?? (object)entity.SessionId!);

                return Result<RecentlyViewedProductsDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recently viewed products.");
                return Result<RecentlyViewedProductsDto>.Failure("Failed to fetch recently viewed products.");
            }
        }
    }
}
