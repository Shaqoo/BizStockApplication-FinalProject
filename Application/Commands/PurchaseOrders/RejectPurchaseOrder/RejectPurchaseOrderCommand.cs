using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.PurchaseOrders.RejectPurchaseOrder
{
    public record RejectPurchaseOrderCommand(
    Guid PurchaseOrderId,
    RejectPurchaseOrderDto RejectPurchaseOrderDto,
    RequestMetadata RequestMetadata
) : IRequest<Result<bool>>;

}
