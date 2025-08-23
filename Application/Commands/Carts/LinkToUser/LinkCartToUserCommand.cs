using Application.Dto;
using MediatR;

namespace Application.Commands.Carts.LinkToUser
{
    public record LinkCartToUserCommand(Guid UserId,string SessionId) : IRequest<Result<string>>;

}
