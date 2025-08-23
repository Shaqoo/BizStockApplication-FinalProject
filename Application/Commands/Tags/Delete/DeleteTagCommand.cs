using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Tags.Delete
{
    public record DeleteTagCommand(Guid Id, RequestMetadata RequestMetadata) : IRequest<Result<string>>;

}
