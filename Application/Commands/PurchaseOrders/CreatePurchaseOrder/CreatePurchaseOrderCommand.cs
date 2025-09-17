using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.PurchaseOrders.CreatePurchaseOrder
{
    public record CreatePurchaseOrderCommand(CreatePurchaseOrderDto CreatePurchaseOrderDto,RequestMetadata RequestMetadata)
         : IRequest<Result<Guid>>;
}
