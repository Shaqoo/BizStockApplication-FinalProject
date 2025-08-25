using Application.Commands.AI;
using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AiController> _logger;

        public AiController(IMediator mediator, ILogger<AiController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Sends a question to the AI and returns its response.
        /// </summary>
        /// <param name="request">The AI message request model.</param>
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] SendAiMessageRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Received a null AI message request.");
                return BadRequest("Request cannot be null.");
            }

            _logger.LogInformation("Received AI message request from UserId: {UserId}", request.UserId);

            var command = new SendAiMessageCommand(request);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("AI message request failed for UserId: {UserId}. Error: {Error}",
                    request.UserId, result.Message);
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
