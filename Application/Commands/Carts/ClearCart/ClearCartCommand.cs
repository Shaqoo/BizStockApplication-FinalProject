using Application.Dto;
using MediatR;

namespace Application.Commands.Carts.ClearCart
{
    public record ClearCartCommand(Guid CartId) : IRequest<Result<CartDto>>;
}
