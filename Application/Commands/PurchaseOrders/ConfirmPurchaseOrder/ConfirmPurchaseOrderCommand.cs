using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.PurchaseOrders.ConfirmPurchaseOrder
{
    public record ConfirmPurchaseOrderCommand(
    Guid PurchaseOrderId,
    ConfirmPurchaseOrderDto ConfirmPurchaseOrderDto,
    RequestMetadata RequestMetadata
) : IRequest<Result<bool>>;
}
