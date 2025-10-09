using Application.Dto;
using MediatR;

namespace Application.Commands.Users.RequestRegenerateMfa
{
    public record RequestMfaRenerateCommand() : IRequest<Result<string>>;

}
