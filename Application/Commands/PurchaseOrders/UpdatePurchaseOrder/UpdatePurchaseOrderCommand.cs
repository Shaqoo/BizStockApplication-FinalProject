using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.PurchaseOrders.UpdatePurchaseOrder
{

    public record UpdatePurchaseOrderCommand(UpdatePurchaseOrderDto Dto, RequestMetadata Metadata)
        : IRequest<Result<Guid>>;

}
