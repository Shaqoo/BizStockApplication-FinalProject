using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Specifications.UpdateSpecification
{
    public record UpdateSpecificationCommand(UpdateSpecificationRequest Request) : IRequest<Result<string>>;
}
