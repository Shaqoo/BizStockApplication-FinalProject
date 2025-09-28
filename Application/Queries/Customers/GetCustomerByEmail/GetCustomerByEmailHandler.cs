using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Customers.GetCustomerByEmail
{
    public class GetCustomerByEmailHandler(IMemoryCacheService memoryCacheService,
        ICustomerRepository customerRepository) : IRequestHandler<GetCustomerByEmailQuery, Result<CustomerDto>>
    {
        public async Task<Result<CustomerDto>> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetCustomerByEmailQuery:{request.email}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var result = await customerRepository.GetByEmailAsync(request.email);
                return result;
            }, TimeSpan.FromMinutes(10));

            if (cachedResult != null)
                return Result<CustomerDto>.Success(cachedResult.CustomerAsDto());
            else
                return Result<CustomerDto>.Failure("Customer Not Found");
        }
    }
}
