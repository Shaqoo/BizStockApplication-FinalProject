using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrdersByDateRange
{
    public record GetPurchaseOrdersByDateRangeQuery(DateTime StartDate,DateTime EndDate,PageRequest PageRequest)
        : IRequest<Result<PaginatedList<PurchaseOrderListDto>>>;
}
