using Application.Dto;
using MediatR;

namespace Application.Queries.Specifications.GetSpecificationById
{
    public record GetSpecificationByIdQuery(Guid Id) : IRequest<Result<SpecificationDto>>;

}
