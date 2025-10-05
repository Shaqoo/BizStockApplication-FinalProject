using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.SalesOrders.GetSalesOrdersByUser
{
    public record GetSalesOrdersByCustomerQuery(PageRequest PageRequest) : IRequest<Result<PaginatedList<SalesOrderDto>>>;

}
