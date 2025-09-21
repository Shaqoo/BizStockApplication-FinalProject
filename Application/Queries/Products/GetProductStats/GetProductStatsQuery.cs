using Application.Dto;
using MediatR;

namespace Application.Queries.Products.GetProductStats
{
    public record GetProductStatsQuery() : IRequest<Result<ProductStatsDto>>;

}
