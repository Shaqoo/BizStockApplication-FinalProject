using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.PurchaseOrders.ReceivePurchaseOrderItems
{
    public record ReceivePurchaseOrderItemsCommand(
    Guid PurchaseOrderId,
    List<ReceivePurchaseOrderItemDto> Items,
    RequestMetadata RequestMetadata
) : IRequest<Result<bool>>;

}
