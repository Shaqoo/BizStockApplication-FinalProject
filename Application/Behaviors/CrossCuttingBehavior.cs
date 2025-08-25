using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors
{
    public class CrossCuttingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
    {
        private readonly ILogger<CrossCuttingBehavior<TRequest, TResponse>> _logger;
        public CrossCuttingBehavior(ILogger<CrossCuttingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var startTime = DateTime.UtcNow;
            try
            {
                _logger.LogInformation("[START] Handling {RequestName} at {StartTime}", requestName, startTime);
                var response = await next();

                var endTime = DateTime.UtcNow;

                _logger.LogInformation("[END] Handled {RequestName} at {EndTime} (Duration: {Duration} ms)", 
                    requestName, endTime, (endTime - startTime).TotalMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging request {RequestName} at {StartTime}", requestName, startTime);
                throw; 
            }
        }
    }
}
