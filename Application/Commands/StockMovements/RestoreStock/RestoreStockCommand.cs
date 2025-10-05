using Application.Commands.StockMovements.ReserveStock;
using Application.Dto;
using Domain.DomainEvents;
using MediatR;

namespace Application.Commands.StockMovements.RestoreStock
{
    public record RestoreStockCommand(
    Guid SalesOrderId,     
    IReadOnlyList<StockItemDto> Items
) : IRequest<Result<Unit>>;

}
