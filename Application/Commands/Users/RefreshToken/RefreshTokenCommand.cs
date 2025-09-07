using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Users.RefreshToken
{
    public record RefreshTokenCommand(RefreshTokenDto TokenDto) : IRequest<Result<AuthDto>>;
    
}
