using Application.Dto;
using MediatR;

namespace Application.Queries.DeliveryAssignments.GetBySalesOrderId
{
    public record GetDeliveryAssignmentByOrderIdQuery(Guid orderId)
    : IRequest<Result<DeliveryAssignmentDto>>;

}
