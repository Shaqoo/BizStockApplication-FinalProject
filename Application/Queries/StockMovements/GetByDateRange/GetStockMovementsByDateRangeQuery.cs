using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.StockMovements.GetByDateRange
{
    public record GetStockMovementsByDateRangeQuery(DateTime StartDate,DateTime EndDate,PageRequest PageRequest)
        : IRequest<Result<PaginatedList<StockMovementDto>>>;
     
}
