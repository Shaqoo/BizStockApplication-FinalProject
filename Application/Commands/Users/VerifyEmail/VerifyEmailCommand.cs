using Application.Dto;
using MediatR;

namespace Application.Commands.Users.VerifyEmail
{
    public record VerifyEmailCommand(string Email, string Token) : IRequest<Result<string>>;

}
