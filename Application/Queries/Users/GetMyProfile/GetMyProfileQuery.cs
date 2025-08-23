using Application.Dto;
using MediatR;

namespace Application.Queries.Users.GetMyProfile
{
    public record GetMyProfileQuery() : IRequest<Result<UserDto>>;
     
}
