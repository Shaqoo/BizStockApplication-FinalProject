using Application.Dto;
using MediatR;

namespace Application.Queries.Refunds.GetBySalesOrderId
{
    public record GetRefundBySalesOrderIdQuery(Guid orderId) : IRequest<Result<IEnumerable<RefundDto>>>;

}
