using Application.Dto;
using MediatR;

namespace Application.Queries.DeliveryAssignments.GetById
{
    public record GetDeliveryAssignmentByIdQuery(Guid assignmentId)
    : IRequest<Result<DeliveryAssignmentDto>>;

}
