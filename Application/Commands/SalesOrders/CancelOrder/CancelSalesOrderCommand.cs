using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.SalesOrders.CancelOrder
{
    public record CancelSalesOrderCommand(Guid SalesOrderId,RequestMetadata RequestMetadata) : IRequest<Result<string>>;

}
