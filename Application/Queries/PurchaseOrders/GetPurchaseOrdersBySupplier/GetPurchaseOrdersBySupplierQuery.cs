using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrdersBySupplier
{
    public record GetPurchaseOrdersBySupplierQuery(
    Guid SupplierId,
    PageRequest PageRequest
) : IRequest<PaginatedList<PurchaseOrderListDto>>;

}
