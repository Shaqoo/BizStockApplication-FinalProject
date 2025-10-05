using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Refunds.GetAll
{
    public class GetAllRefundsQueryHandler(IMemoryCacheService memoryCacheService,
        IRefundRepository refundRepository) : IRequestHandler<GetAllRefundsQuery, Result<PaginatedList<RefundDto>>>
    {
        public async Task<Result<PaginatedList<RefundDto>>> Handle(GetAllRefundsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Refunds_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var paginatedRefunds = await refundRepository.GetAllAsync(request.PageRequest);
                var refundDtos = paginatedRefunds.Items.Select(refund => new RefundDto
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
                }).ToList();
                return new PaginatedList<RefundDto>(refundDtos, paginatedRefunds.TotalCount, request.PageRequest.Page, request.PageRequest.PageSize);
            });

            return cachedResult != null ? Result<PaginatedList<RefundDto>>.Success(cachedResult) :
                Result<PaginatedList<RefundDto>>.Failure("Failed to retrieve refunds from cache or database.");

        }
    }
}
