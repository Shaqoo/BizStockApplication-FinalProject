using Application.Dto;
using MediatR;

namespace Application.Queries.StockMovements.GetStockMovementTrend
{
    public record GetStockMovementTrendQuery(string Range) : IRequest<Result<List<StockMovementTrendDto>>>;

}
