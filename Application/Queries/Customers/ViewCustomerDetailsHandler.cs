using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Customers
{
    public class ViewCustomerDetailsHandler(IAuthService authService,
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<ViewCustomerDetailsQuery, Result<CustomerDto>>
    {
        public async Task<Result<CustomerDto>> Handle(ViewCustomerDetailsQuery request, CancellationToken cancellationToken)
        {
             var currentUser = authService.CurrentUser();
            if (currentUser is null)
                return Result<CustomerDto>.Failure("User not found.");

            var checkIfExists = await userRepository.CheckIfExists(x => x.Id == currentUser.Id && !x.IsDeleted);
            if (!checkIfExists)
                return Result<CustomerDto>.Failure("User not found.");

            var cacheKey = $"CustomerDetails:{currentUser.Id}";
            var customerDto = await distributedCacheService.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var customer = await customerRepository.GetByEmailAsync(currentUser.Email);
                    if (customer is null)
                        return null!;
                    return customer.CustomerAsDto();
                },
                TimeSpan.FromMinutes(10)  
            );
            if (customerDto is null)
                return Result<CustomerDto>.Failure("Customer not found.");

            return Result<CustomerDto>.Success(customerDto);
        }
    }
}
