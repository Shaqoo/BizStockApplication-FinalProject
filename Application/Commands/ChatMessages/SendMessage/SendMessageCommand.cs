using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.ChatMessages.SendMessage
{
    public record SendMessageCommand(SendMessageRequest SendMessageRequest,RequestMetadata RequestMetadata) : IRequest<Result<MessageDto>>;

}
