using Application.Dto;
using MediatR;

namespace Application.Queries.Specifications.GetAllSpecifications
{
    public record GetAllSpecificationsQuery() : IRequest<Result<List<SpecificationDto>>>;
}
