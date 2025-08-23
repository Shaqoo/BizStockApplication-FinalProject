using Application.Dto;
using MediatR;

namespace Application.Queries.Users.GetByEmail
{
    public record GetUserByEmailQuery(string Email) : IRequest<Result<UserDto>>;
     
}
