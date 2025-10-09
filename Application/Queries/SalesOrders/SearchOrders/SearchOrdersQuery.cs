using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.SalesOrders.SearchOrders
{
    public record SearchOrdersQuery(string query,PageRequest PageRequest) 
        : IRequest<Result<PaginatedList<SalesOrderDto>>>;

}
