using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Specifications.UpdateProductSpecification
{
    public record UpdateProductSpecificationCommand(UpdateProductSpecificationRequest Request) : IRequest<Result<string>>;
}
