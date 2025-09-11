using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Enums;
using MediatR;

namespace Application.Queries.Customers.GetCustomerStats
{
    public class GetCustomerStatsHandler(IUserRepository customerRepository,
        IChatThreadRepository chatThreadRepository,
        ISalesOrderRepository orderRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetCustomerStatsQuery, Result<CustomerStatsDto>>
    {
        public async Task<Result<CustomerStatsDto>> Handle(GetCustomerStatsQuery request, CancellationToken cancellationToken)
        {
             string cacheKey = "customer_stats";

             var cachedStats = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
             {
                 var totalCustomers = await customerRepository.CountAsync(a => a.UserRoles.Any(a => a.Role == Role.Customer));
                 var verifiedCustomers = await customerRepository.CountAsync(c => c.UserRoles.Any(a => a.Role == Role.Customer) &&
                 c.IsEmailVerified);
                 var totalOrders = await orderRepository.CountByStatusAsync(OrderStatus.Delivered);
                 var openComplaints = await chatThreadRepository.CountOpenThreadsAsync();
                 return new CustomerStatsDto
                 {
                     TotalCustomers = totalCustomers,
                     VerifiedCustomers = verifiedCustomers,
                     TotalOrders = totalOrders,
                     OpenComplaints = openComplaints
                 };
             }, TimeSpan.FromMinutes(10));

            return Result<CustomerStatsDto>.Success(cachedStats);
        }
    }
}
