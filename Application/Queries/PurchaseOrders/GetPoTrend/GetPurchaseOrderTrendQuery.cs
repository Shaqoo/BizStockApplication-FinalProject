using Application.Dto;
using MediatR;

namespace Application.Queries.PurchaseOrders.GetPoTrend
{
    public record GetPurchaseOrderTrendQuery() : IRequest<Result<PoTrendDto>>;
}
