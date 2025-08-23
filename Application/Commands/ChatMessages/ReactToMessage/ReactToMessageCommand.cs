using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.ChatMessages.ReactToMessage
{
    public record ReactToMessageCommand(ReactToMessageRequest ReactToMessageRequest) : IRequest<Result<string>>;

}
