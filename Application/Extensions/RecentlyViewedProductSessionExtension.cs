using Microsoft.AspNetCore.Http;

namespace Application.Extensions
{
    public static class RecentlyViewedProductSessionExtension
    {
        public const string RecentlyViewedProductSessionKey = "RecentlyViewedSession";

        public static string GetOrAddRecentlyViewedProductSession(this HttpContext httpContext)
        {
            if(httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if(httpContext.Request.Cookies.TryGetValue(RecentlyViewedProductSessionKey,out var sessionId)
                && !string.IsNullOrWhiteSpace(sessionId))
            {
                return sessionId;
            }

            sessionId = Guid.NewGuid().ToString();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddMonths(1),
                Secure = httpContext.Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                IsEssential = true,
            };

            httpContext.Response.Cookies.Append(RecentlyViewedProductSessionKey,sessionId, cookieOptions);

            return sessionId;
        }

        public static void ClearRecentlyViewedProductSessionId(this HttpContext httpContext)
        {
            if (httpContext is null)
                throw new ArgumentNullException(nameof(httpContext));

            httpContext.Response.Cookies.Delete(RecentlyViewedProductSessionKey);
        }
    }
}
