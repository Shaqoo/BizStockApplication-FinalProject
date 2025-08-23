using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Products.AddQuantity
{
    public record AddProductQuantityCommand(
    AddProductQuantityDto Dto,
    RequestMetadata Metadata
) : IRequest<Result<string>>;

}
