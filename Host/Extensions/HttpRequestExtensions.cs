using Application.Dto.RequestModels;

namespace Host.Extensions
{
    public static class HttpRequestExtensions
    {
        public static RequestMetadata GetRequestMetadata(this HttpRequest request)
        {
            return new RequestMetadata(request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                request.Headers["User-Agent"].ToString());
             
        }
    }
}
