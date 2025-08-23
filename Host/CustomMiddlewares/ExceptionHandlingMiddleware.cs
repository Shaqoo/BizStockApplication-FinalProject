using Application.Dto;
using Domain.Exceptions;

namespace Host.CustomMiddlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BizStockException bizStockEx)
            {
                _logger.LogWarning(bizStockEx, "Domain/Repository Exception");
                context.Response.StatusCode = bizStockEx.StatusCode;

                var error = new ErrorDto
                {
                    StatusCode = bizStockEx.StatusCode,
                    Message = bizStockEx.Message,
                    Source = "Domain/Repository",
                    Timestamp = DateTime.UtcNow,
                    TraceId = context.TraceIdentifier
                };

                await context.Response.WriteAsJsonAsync(error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");
                context.Response.StatusCode = 500;

                var error = new ErrorDto
                {
                    Source = "Unhandled",
                    Message = ex.Message,
                    StatusCode = 500,
                    TraceId = context.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
