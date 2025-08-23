using Application.Dto;
using Application.Interfaces.Service;
using Infrastructures.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructures.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IOptions<JwtSettings> jwtSettings, IHttpContextAccessor httpContextAccessor) 
        {
            _jwtSettings = jwtSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid CurrentUserId =>
    Guid.TryParse(
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
        out var id
    ) ? id : Guid.Empty;
        
        private string CurrentUserEmail =>
            _httpContextAccessor.HttpContext?.User?.FindFirst("Email")?.Value ?? string.Empty;

        private string CurrentUserRole =>
            _httpContextAccessor.HttpContext?.User?.FindFirst("Role")?.Value ?? string.Empty;



        public string GenerateToken(UserDto model)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new("Email",model.Email),
                new("Role",model.Role),
                new(ClaimTypes.NameIdentifier,model.Id.ToString()),
                new(ClaimTypes.Name, model.fullName),
                new("phoneNumber", model.phoneNumber),
                new("Dob", model.Dob.ToString("o"))
            };
            var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, null, DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.AccessTokenExpirationMinutes)), credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

         public CurrentUserDto? CurrentUser()
         {
            if (CurrentUserId == Guid.Empty)
            {
                return null;
            }
            if (string.IsNullOrEmpty(CurrentUserEmail) && string.IsNullOrEmpty(CurrentUserRole))
            {
                return null;
            }
            Console.WriteLine(CurrentUserRole);
            Console.WriteLine(CurrentUserEmail);
            return new CurrentUserDto(CurrentUserId,CurrentUserEmail,CurrentUserRole);
        }

        public string GenerateTempJwt(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            if (key is null)
                throw new Exception("Key Is Null");
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim("mfa", "required")
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),  
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public ClaimsPrincipal ValidateTempJwt(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var tokenHandler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.FromSeconds(10)
            };

            return tokenHandler.ValidateToken(token, parameters, out _);
        }

    }
}
