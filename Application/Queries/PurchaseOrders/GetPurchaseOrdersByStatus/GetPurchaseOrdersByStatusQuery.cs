using Application.Dto;
using Application.Pagination;
using Domain.Enums;
using MediatR;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrdersByStatus
{
    public record GetPurchaseOrdersByStatusQuery(
    PurchaseOrderStatus Status,
    PageRequest PageRequest
) : IRequest<PaginatedList<PurchaseOrderListDto>>;

}
