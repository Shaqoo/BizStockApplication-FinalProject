using Application.Dto;
using MediatR;

namespace Application.Queries.Reviews.GetRatingByProduct
{
    public record GetRatingByProductIdQuery(Guid ProductId) : IRequest<Result<Tuple<double,int>>>;
}
