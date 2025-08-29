namespace Host.CustomMiddlewares
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var csp = string.Join(" ",
                "default-src 'self';",
                "script-src 'self' https://apis.google.com https://connect.facebook.net;",
                "object-src 'none';",
                "base-uri 'self';",
                "frame-ancestors 'none';",
                "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com;",
                "font-src 'self' https://fonts.gstatic.com;",
                "connect-src 'self' https://api.yourdomain.com http://localhost:5500 https://accounts.google.com https://login.microsoftonline.com https://github.com https://facebook.com;",
                "img-src 'self' data: https://githubusercontent.com https://*.fbcdn.net;",
                "frame-src 'self' https://accounts.google.com https://login.microsoftonline.com https://github.com https://facebook.com;"
            );

            context.Response.Headers.Append("Content-Security-Policy", csp);
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("Permissions-Policy", "geolocation=(), camera=(), microphone=()");
            context.Response.Headers.Append("X-XSS-Protection", "0");

            await _next(context);
        }
    }
}
