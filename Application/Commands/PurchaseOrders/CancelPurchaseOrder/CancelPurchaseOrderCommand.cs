using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.PurchaseOrders.CancelPurchaseOrder
{
    public record CancelPurchaseOrderCommand(
    Guid PurchaseOrderId,
    CancelPurchaseOrderDto CancelPurchaseOrderDto,
    RequestMetadata RequestMetadata
) : IRequest<Result<bool>>;

}
