using Application.Dto;
using MediatR;

namespace Application.Queries.SalesOrders.GetSalesOrderById
{
    public record GetSalesOrderByIdQuery(Guid salesOrderId) : IRequest<Result<SalesOrderDto>>;

}
