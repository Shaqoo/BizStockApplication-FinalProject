using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Payments.GetPaymentsByCustomer
{
    public class GetPaymentsByCustomerHandler(IMemoryCacheService memoryCacheService,
        IPaymentRepository paymentRepository) : IRequestHandler<GetPaymentsByCustomerQuery, Result<PaginatedList<PaymentDto>>>
    {
        public async Task<Result<PaginatedList<PaymentDto>>> Handle(GetPaymentsByCustomerQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetPaymentsByCustomerQuery:CustomerId:{request.CustomerId}:Page:{request.PageRequest.Page}:PageSize:{request.PageRequest.PageSize}";

            var cahedResult = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var result = await paymentRepository.GetByCustomerIdAsync(request.CustomerId, request.PageRequest);
                    return result;
                },TimeSpan.FromMinutes(5));

            return Result<PaginatedList<PaymentDto>>.Success(cahedResult ?? new PaginatedList<PaymentDto>());
        }
    }
}
