using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Users.VerifyPassword
{
    public record VerifyPasswordResetCodeCommand(VerifyPasswordReset VerifyPassword) : IRequest<Result<string>>;

}
