using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.SalesOrders.GetSalesOrdersByCustomerId
{
    public record GetSalesOrderByCustomerIdQuery(Guid customerId,PageRequest PageRequest) 
        : IRequest<Result<PaginatedList<SalesOrderDto>>>;

}
