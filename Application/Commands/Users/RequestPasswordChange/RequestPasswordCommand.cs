using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Users.RequestPasswordChange
{
    public record RequestPasswordCommand(RequestPasswordRequest Request, RequestMetadata RequestMetadata) : IRequest<Result<string>>;

}
     
