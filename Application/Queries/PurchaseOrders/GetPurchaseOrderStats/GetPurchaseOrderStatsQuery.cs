using Application.Dto;
using MediatR;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrderStats
{
    public record GetPurchaseOrderStatsQuery()
    : IRequest<Result<PurchaseOrderStatsDto>>;

}
