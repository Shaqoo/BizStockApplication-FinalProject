using Application.Dto;
using MediatR;

namespace Application.Queries.StockMovements.GetStats
{
    public record GetStockMovementsStatsQuery() : IRequest<Result<StockMovementStatsDto>>;

}
