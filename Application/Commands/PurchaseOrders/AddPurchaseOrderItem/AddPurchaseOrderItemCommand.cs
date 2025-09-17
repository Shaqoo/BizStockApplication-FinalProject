using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.PurchaseOrders.AddPurchaseOrderItem
{
    public record AddPurchaseOrderItemCommand(AddPurchaseOrderItemDto Dto, RequestMetadata Metadata)
    : IRequest<Result<Guid>>;
}
