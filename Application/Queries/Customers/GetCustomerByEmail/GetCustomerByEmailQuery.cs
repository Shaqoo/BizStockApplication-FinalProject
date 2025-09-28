using Application.Dto;
using MediatR;

namespace Application.Queries.Customers.GetCustomerByEmail
{
    public record GetCustomerByEmailQuery(string email) : IRequest<Result<CustomerDto>>;
     
}
