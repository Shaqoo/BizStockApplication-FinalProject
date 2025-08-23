using Application.Dto;
using Application.Pagination;
using Domain.Enums;
using MediatR;

namespace Application.Queries.StockMovements.GetByMovementType
{
    public record GetStockMovementsByMovementTypeQuery(StockMovementType MovementType, PageRequest PageRequest) 
        : IRequest<Result<PaginatedList<StockMovementDto>>>;
        
}


