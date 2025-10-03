using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.SalesOrders.Create
{
    public record CreateSalesOrderCommand(CreateSalesOrderRequestModel CreateSalesOrderRequestModel,
        RequestMetadata RequestMetadata)
        :IRequest<Result<Guid>>;
     
}
