using Application.Dto;
using MediatR;

namespace Application.Queries.Customers.GetCustomerStats
{
    public record GetCustomerStatsQuery : IRequest<Result<CustomerStatsDto>>;

}
