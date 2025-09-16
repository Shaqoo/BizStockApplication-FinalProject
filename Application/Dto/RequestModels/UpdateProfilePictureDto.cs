using Microsoft.AspNetCore.Http;

namespace Application.Dto.RequestModels
{
    public record UpdateProfilePictureDto(IFormFile File);
     
}
