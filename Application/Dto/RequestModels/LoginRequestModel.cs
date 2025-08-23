using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public record LoginRequestModel(
     string Email,
     string Password,
     string? CaptchaToken
 );

}
