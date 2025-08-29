using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Specifications.AddProductSpecification
{
    public record AddProductSpecificationCommand(AddProductSpecificationRequest Request) : IRequest<Result<Guid>>;
}
