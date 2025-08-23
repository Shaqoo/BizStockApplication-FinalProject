using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IAuthService
    {
        string GenerateToken(UserDto model);
        string GenerateRefreshToken();
        CurrentUserDto? CurrentUser();
        string GenerateTempJwt(string userId);
        ClaimsPrincipal ValidateTempJwt(string token);
    }
}
