using Application.Dto;
using Application.Dto.RequestModels;
using Domain.Enums;
using MediatR;

namespace Application.Commands.Refunds.ProcessRefund
{
    public record ProcessRefundCommand(
       Guid SalesOrderId,
       decimal Amount,
       PaymentMethod PaymentMethod,
       string Reason,
       string ReferenceNo,
       RequestMetadata RequestMetadata
   ) : IRequest<Result<Guid>>;
}
