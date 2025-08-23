using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public record TokenRequestModel
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }

}
