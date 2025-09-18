using Application.Dto;
using MediatR;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrderById
{
    public record GetPurchaseOrderByIdQuery(Guid PurchaseOrderId)
    : IRequest<Result<PurchaseOrderDetailDto>>;

}
