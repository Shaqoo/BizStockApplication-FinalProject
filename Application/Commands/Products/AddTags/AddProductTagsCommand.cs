using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Products.AddTags
{
    public record AddProductTagsCommand(AddProductTagDto addProductTag,RequestMetadata requestMetadata)
        : IRequest<Result<string>>;
     
}
