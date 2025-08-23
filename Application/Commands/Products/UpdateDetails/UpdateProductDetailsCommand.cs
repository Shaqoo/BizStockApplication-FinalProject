using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Products.UpdateDetails
{
    public record UpdateProductDetailsCommand(Guid ProductId,
        UpdateProductDetailsDto ProductDetails,
        RequestMetadata RequestMetadata) : IRequest<Result<string>>;
    
      
}
