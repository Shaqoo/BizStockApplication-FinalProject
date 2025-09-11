using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.ChatThreads.Create
{
    public record CreateChatThreadCommand(RequestMetadata RequestMetadata) : IRequest<Result<Guid>>;
     
}
