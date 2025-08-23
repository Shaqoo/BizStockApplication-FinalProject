using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Users.UpdateProfilePicture
{
    public record UpdateUserPictureCommand(UpdateProfilePictureDto UpdateProfilePicture,RequestMetadata RequestMetadata) : IRequest<Result<string>>;
     
}
