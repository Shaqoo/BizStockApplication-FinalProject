using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Specifications.CreateSpecification
{
    public record CreateSpecificationCommand(CreateSpecificationRequest Request) : IRequest<Result<Guid>>;
}
