using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Products.Create
{
    public record CreateProductCommand(CreateProductRequestModel RequestModel,RequestMetadata RequestMetadata) 
        : IRequest<Result<ProductDto>>;
}
