using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Payments.GetPaymentsByCustomer
{
    public class GetPaymentsByCustomerHandler(IMemoryCacheService memoryCacheService,
        ICustomerRepository customerRepository,
        IUserRepository userRepository,
        IPaymentRepository paymentRepository) : IRequestHandler<GetPaymentsByCustomerQuery, Result<PaginatedList<PaymentDto>>>
    {
        public async Task<Result<PaginatedList<PaymentDto>>> Handle(GetPaymentsByCustomerQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetPaymentsByCustomerQuery:CustomerId:{request.CustomerId}:Page:{request.PageRequest.Page}:PageSize:{request.PageRequest.PageSize}";

            var cahedResult = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var user = await userRepository.GetByIdAsync(request.CustomerId);
                    var customer = await customerRepository.GetByEmailAsync((string)user!.Email);
                    var result = await paymentRepository.GetByCustomerIdAsync(customer!.Id, request.PageRequest);
                    return result;
                },TimeSpan.FromMinutes(5));

            return Result<PaginatedList<PaymentDto>>.Success(cahedResult ?? new PaginatedList<PaymentDto>());
        }
    }
}
