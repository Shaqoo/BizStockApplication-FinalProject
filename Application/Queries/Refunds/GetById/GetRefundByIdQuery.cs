using Application.Dto;
using MediatR;

namespace Application.Queries.Refunds.GetById
{
    public record GetRefundByIdQuery(Guid refundId) : IRequest<Result<RefundDto>>;
   

}
