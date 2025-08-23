using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public record UpdateProductPictureDto(Guid ProductId,
        IFormFile Picture);
    
}
