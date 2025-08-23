using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.ExternalLogin.Microsoft
{
    public record MicrosoftLoginCommand(ExternalLoginDto dto, RequestMetadata RequestMetadata) : IRequest<Result<AuthDto>>;
     
}
