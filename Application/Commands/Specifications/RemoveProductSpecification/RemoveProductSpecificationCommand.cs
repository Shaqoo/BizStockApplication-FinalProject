using Application.Dto;
using MediatR;

namespace Application.Commands.Specifications.RemoveProductSpecification
{
    public record RemoveProductSpecificationCommand(Guid ProductSpecificationId) : IRequest<Result<string>>;
}
