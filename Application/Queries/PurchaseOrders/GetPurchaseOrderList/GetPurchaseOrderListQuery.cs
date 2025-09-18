using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrderList
{
    public record GetPurchaseOrderListQuery(PageRequest PageRequest)
    : IRequest<PaginatedList<PurchaseOrderListDto>>;

}
