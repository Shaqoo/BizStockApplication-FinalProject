using Application.Configurations;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.SalesOrders.TrackItem
{
    public class TrackItemQueryHandler(IFezService fezService,
        IMemoryCacheService memoryCacheService) : IRequestHandler<TrackItemQuery, Result<TrackOrderResponseDto>>
    {
        public async Task<Result<TrackOrderResponseDto>> Handle(TrackItemQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"TrackItem_{request.trackingNumber}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var trackResult = await fezService.TrackOrderAsync(request.trackingNumber);
                if (!trackResult.Success || trackResult.Data is null)
                {
                    return Result<TrackOrderResponseDto>.Failure(trackResult.Message);
                }
                FezHelper.UpdateOrderHistory(trackResult.Data);
                return Result<TrackOrderResponseDto>.Success(trackResult.Data);
            }, TimeSpan.FromMinutes(10));

            return cachedResult ?? Result<TrackOrderResponseDto>.Failure("Unable to track item at this time.");
        }
    }
}
