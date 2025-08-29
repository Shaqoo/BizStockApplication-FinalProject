using Application.Dto;
using MediatR;

namespace Application.Queries.Reviews.GetProductReviewSummaryQuery
{
    public record GetProductReviewSummaryQuery(Guid ProductId) : IRequest<Result<RatingSummaryDto>>;
 
}
