using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Reviews.GetProductReviewSummaryQuery
{
    public class GetProductReviewSummaryHandler(IReviewRepository reviewRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetProductReviewSummaryQuery, Result<RatingSummaryDto>>
    {
        public async Task<Result<RatingSummaryDto>> Handle(GetProductReviewSummaryQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetProductReviewSummaryQuery:{request.ProductId}";

            var result = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var result = await reviewRepository.GetProductRatingSummaryAsync(request.ProductId);
                    return Result<RatingSummaryDto>.Success(result);
                });
            return result;
        }
    }
}
