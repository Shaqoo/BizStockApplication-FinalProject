using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Reviews.GetReviewsByProduct
{
    public class GetReviewsForProductHandler(IReviewRepository reviewRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<GetReviewsForProductIdQuery, Result<PaginatedList<ProductReviewDto>>>
    {
        public async Task<Result<PaginatedList<ProductReviewDto>>> Handle(GetReviewsForProductIdQuery request, CancellationToken cancellationToken)
        {
             string cacheKey = $"ProductReviews_{request.ProductId}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";
             
             var cachedReviews = await distributedCacheService.GetOrAddAsync(cacheKey, async () =>
             {
                 var reviews = await reviewRepository.GetByProductIdAsync(request.ProductId, request.PageRequest);
                 return reviews;
             });

            return Result<PaginatedList<ProductReviewDto>>.Success(
                new PaginatedList<ProductReviewDto>(cachedReviews.Items.Select(review => new ProductReviewDto(
                    review.Id,
                    review.ProductId ?? Guid.Empty,
                    new ReviewUserDto(review.ReviewerId,review.Reviewer.FullName,review.Reviewer.ProfilePictureUrl),
                    review.Comment,
                    review.Rating,
                    review.ReviewedAt
                )).ToList(),
                cachedReviews.TotalCount,
                cachedReviews.PageNumber,
                cachedReviews.PageSize
            ));
        }
    }
}
