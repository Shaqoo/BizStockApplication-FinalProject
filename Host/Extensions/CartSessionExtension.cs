using Microsoft.AspNetCore.Http;
using System.Text;

namespace Host.Extensions
{
    public static class CartSessionExtension
    {
        private const string CartSessionKey = "CartSession";

        public static string GetOrCreateCartSessionId(this HttpContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));


            if (context.Request.Cookies.TryGetValue(CartSessionKey, out var sessionId) && 
                !string.IsNullOrWhiteSpace(sessionId))
            {
                return sessionId;
            }

            sessionId = Guid.NewGuid().ToString();
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,  
                SameSite = SameSiteMode.Strict,  
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                IsEssential = true,
                MaxAge = TimeSpan.FromDays(30)
            };

            context.Response.Cookies.Append(CartSessionKey, sessionId, cookieOptions);

            return sessionId;
        }

        public static void ClearCartSessionId(this HttpContext httpContext)
        {
            if (httpContext is null)
                throw new ArgumentNullException(nameof(httpContext));

            httpContext.Response.Cookies.Delete(CartSessionKey);
        }
    }
}
