using Application.Dto;
using MediatR;

namespace Application.Commands.Users.SendEmailVerificationToken
{
    public record SendEmailVerificationTokenCommand(string Email) : IRequest<Result<string>>;

}
