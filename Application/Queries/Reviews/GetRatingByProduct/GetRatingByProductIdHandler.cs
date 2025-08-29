using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Reviews.GetRatingByProduct
{
    public class GetRatingByProductIdHandler(IReviewRepository reviewRepository,
        IMemoryCacheService memoryCacheService)
        : IRequestHandler<GetRatingByProductIdQuery, Result<Tuple<double,int>>>
    {
        public async Task<Result<Tuple<double,int>>> Handle(GetRatingByProductIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetRatingByProductIdQuery:{request.ProductId}";

            var result = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var rating = await reviewRepository.GetAverageRatingForProductAsync(request.ProductId);
                    var total = await reviewRepository.TotalRatingForAProductAsync(request.ProductId);
                    return new Tuple<double, int>(rating,total);
                },TimeSpan.FromMinutes(5));

            return Result<Tuple<double,int>>.Success(result);
        }
    }
}
