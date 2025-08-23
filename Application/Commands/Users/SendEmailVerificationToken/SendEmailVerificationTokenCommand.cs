using Application.Dto;
using MediatR;

namespace Application.Commands.Users.SendEmailVerificationToken
{
    public record SendEmailVerificationTokenCommand(Guid UserId, string Email) : IRequest<Result<string>>;

}
