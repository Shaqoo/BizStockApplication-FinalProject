using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public class RefreshTokenDto
    {
        /// <summary>
        /// The refresh token previously issued to the user.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
