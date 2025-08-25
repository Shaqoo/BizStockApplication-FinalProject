using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.AI
{
    public record SendAiMessageCommand(SendAiMessageRequest MessageRequest) : IRequest<Result<string>>;

}
