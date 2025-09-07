using Microsoft.AspNetCore.Http;

namespace Application.Extensions
{
    public static class RefreshTokenCookieExtension
    {
        public const string RefreshTokenCookieKey = "RefreshToken";

        /// <summary>
        /// Adds or updates the refresh token cookie.
        /// </summary>
        public static void SetRefreshToken(this HttpResponse response, string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,              // Cannot be accessed by JavaScript (XSS protection)
                Secure = true,                // Only over HTTPS
                SameSite = SameSiteMode.None, // Prevent CSRF attacks
                Expires = DateTimeOffset.UtcNow.AddDays(7) // Lifetime of refresh token
            };

            response.Cookies.Append(RefreshTokenCookieKey, refreshToken, cookieOptions);
        }

        /// <summary>
        /// Retrieves the refresh token from the request cookies.
        /// </summary>
        public static string? GetRefreshToken(this HttpRequest request)
        {
            request.Cookies.TryGetValue(RefreshTokenCookieKey, out var token);
            return token;
        }

        /// <summary>
        /// Clears the refresh token cookie (e.g., on logout).
        /// </summary>
        public static void ClearRefreshToken(this HttpResponse response)
        {
            response.Cookies.Delete(RefreshTokenCookieKey);
        }
    }

}
