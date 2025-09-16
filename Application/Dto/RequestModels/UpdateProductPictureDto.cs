using Microsoft.AspNetCore.Http;

namespace Application.Dto.RequestModels
{
    public record UpdateProductPictureDto(Guid ProductId,
        IFormFile Picture);
    
}
