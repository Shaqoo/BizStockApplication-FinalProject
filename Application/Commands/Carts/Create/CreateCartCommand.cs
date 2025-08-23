using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Carts.Create
{
    public record CreateCartCommand(CreateCartRequest CreateCartRequest) : IRequest<Result<CartDto>>;

}
