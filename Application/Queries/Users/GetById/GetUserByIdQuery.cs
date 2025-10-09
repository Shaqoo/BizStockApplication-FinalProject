using Application.Dto;
using MediatR;

namespace Application.Queries.Users.GetById
{
    public record GetUserByIdQuery(Guid id) : IRequest<Result<UserDto>>;
    
}
