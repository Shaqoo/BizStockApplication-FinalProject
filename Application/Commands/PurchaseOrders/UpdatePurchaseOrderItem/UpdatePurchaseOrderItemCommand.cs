using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.PurchaseOrders.UpdatePurchaseOrderItem
{
    public record UpdatePurchaseOrderItemCommand(UpdatePurchaseOrderItemDto Dto, RequestMetadata Metadata)
    : IRequest<Result<Guid>>;

}
