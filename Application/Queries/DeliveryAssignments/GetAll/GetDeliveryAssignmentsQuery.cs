using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.DeliveryAssignments.GetAll
{
    public record GetDeliveryAssignmentsQuery(PageRequest PageRequest)
    : IRequest<Result<PaginatedList<DeliveryAssignmentDto>>>;

}
