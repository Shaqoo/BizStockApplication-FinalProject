using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.PurchaseOrders.RemovePurchaseOrderItem
{
    public record RemovePurchaseOrderItemCommand(RemovePurchaseOrderItemDto Dto, RequestMetadata Metadata)
    : IRequest<Result<Guid>>;
}
