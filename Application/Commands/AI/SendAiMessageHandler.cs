using Application.Dto;
using Application.Interfaces.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.AI
{
    public class SendAiMessageHandler : IRequestHandler<SendAiMessageCommand, Result<string>>
    {
        private readonly IAiService _aiMessageService;
        private readonly ILogger<SendAiMessageHandler> _logger;

        public SendAiMessageHandler(
            IAiService aiMessageService,
            ILogger<SendAiMessageHandler> logger)
        {
            _aiMessageService = aiMessageService;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(SendAiMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Processing AI message for UserId: {UserId}, Role: {Role}, Question: {Question}",
                    request.MessageRequest.UserId,
                    request.MessageRequest.Role,
                    request.MessageRequest.Question);

                var response = await _aiMessageService.GetResponseAsync(
                    request.MessageRequest.UserId,
                    request.MessageRequest.Role,
                    request.MessageRequest.Question);

                _logger.LogInformation("AI response successfully generated for UserId: {UserId}", request.MessageRequest.UserId);

                return Result<string>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing AI message for UserId: {UserId}", request.MessageRequest.UserId);
                return Result<string>.Failure("An error occurred while generating the AI response.");
            }
        }
    }
}
