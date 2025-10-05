using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Refunds.GetBySalesOrderId
{
    public class GetRefundBySalesOrderIdQueryHandler(IMemoryCacheService memoryCacheService,
        IRefundRepository refundRepository) : IRequestHandler<GetRefundBySalesOrderIdQuery, Result<IEnumerable<RefundDto>>>
    {
        public async Task<Result<IEnumerable<RefundDto>>> Handle(GetRefundBySalesOrderIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Refund_ByOrder_{request.orderId}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var refunds = await refundRepository.GetByOrderIdAsync(request.orderId);
                if (refunds.Count() == 0)
                {
                    return Result<IEnumerable<RefundDto>>.Failure("Refund not found");
                }
                var refundDto = refunds.Select(refund => new RefundDto
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
                });
                return Result<IEnumerable<RefundDto>>.Success(refundDto);
            }, TimeSpan.FromMinutes(20));

            return cachedResult switch
            {
                Result<IEnumerable<RefundDto>> result when result.IsSuccess => result,
                _ => Result<IEnumerable<RefundDto>>.Failure("Failed to retrieve refund details")
            };
        }
    }
}
