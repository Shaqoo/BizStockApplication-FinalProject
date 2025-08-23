using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.StockMovements.GetByProduct
{
    public record GetStockMovementByProductIdQuery(Guid ProductId,PageRequest PageRequest) : IRequest<Result<PaginatedList<StockMovementDto>>>;
    
}
