using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Reviews.GetReviewsByProduct
{
    public record GetReviewsForProductIdQuery(Guid ProductId,PageRequest PageRequest) 
        : IRequest<Result<PaginatedList<ProductReviewDto>>>;
    
}
