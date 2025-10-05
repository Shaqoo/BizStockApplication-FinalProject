using Application.Dto;
using Domain.DomainEvents;
using MediatR;

namespace Application.Commands.StockMovements.ReserveStock
{
    public record ReserveStockCommand(
    Guid SalesOrderId,
    IReadOnlyList<StockItemDto> Items
) : IRequest<Result<Unit>>;


}
