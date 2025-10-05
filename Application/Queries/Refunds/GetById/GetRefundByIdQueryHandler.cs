using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Refunds.GetById
{
    public class GetRefundByIdQueryHandler(IMemoryCacheService memoryCacheService,
        IRefundRepository refundRepository) : IRequestHandler<GetRefundByIdQuery, Result<RefundDto>>
    {
        public async Task<Result<RefundDto>> Handle(GetRefundByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Refund_{request.refundId}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var refund = await refundRepository.GetByIdAsync(request.refundId);
                if (refund == null)
                {
                    return Result<RefundDto>.Failure("Refund not found");
                }
                var refundDto = new RefundDto
                {
                    Id = refund.Id,
                    SalesOrderId = refund.SalesOrderId,
                    Amount = refund.Amount,
                    PaymentMethod = refund.PaymentMethod,
                    TransactionReference = refund.TransactionReference,
                    Status = refund.Status,
                    Reason = refund.Reason,
                    RequestedAt = refund.RequestedAt,
                    CompletedAt = refund.CompletedAt,
                    CustomerId = refund.Order.CustomerId,
                    CustomerName = refund.Order.Customer.FullName,
                    OrderNumber = refund.Order.OrderNumber,
                    RefundReference = refund.RefundReference
                };
                return Result<RefundDto>.Success(refundDto);
            },TimeSpan.FromMinutes(20));

            return cachedResult switch
            {
                Result<RefundDto> result when result.IsSuccess => result,
                _ => Result<RefundDto>.Failure("Failed to retrieve refund details")
            };
        }
    }
}
