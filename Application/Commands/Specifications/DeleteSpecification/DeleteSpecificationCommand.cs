using Application.Dto;
using MediatR;

namespace Application.Commands.Specifications.DeleteSpecification
{
    public record DeleteSpecificationCommand(Guid Id) : IRequest<Result<string>>;
}
