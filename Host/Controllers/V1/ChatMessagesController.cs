using Application.Commands.ChatMessages.MarkAsRead;
using Application.Commands.ChatMessages.ReactToMessage;
using Application.Commands.ChatMessages.SendMessage;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.ChatMessages.GetMessageById;
using Application.Queries.ChatMessages.GetMessagesByThread;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatMessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all messages in a chat thread.
        /// </summary>
        /// <param name="threadId">The unique identifier of the chat thread.</param>
        /// <param name="pageRequest">Pagination parameters (page number, page size).</param>
        /// <returns>A paginated list of messages.</returns>
        [HttpGet("thread/{threadId}")]
        [ProducesResponseType(typeof(Result<PaginatedList<MessageDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMessagesByThreadId(Guid threadId, [FromQuery] PageRequest pageRequest)
        {
            var query = new GetMessagesByThreadIdQuery(threadId, pageRequest);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Get a single message by its ID.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message.</param>
        /// <returns>The message details.</returns>
        [HttpGet("{messageId}")]
        [ProducesResponseType(typeof(Result<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMessageById(Guid messageId)
        {
            var query = new GetMessageByIdQuery(messageId);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Send a new chat message.
        /// </summary>
        /// <param name="request">The request containing message details.</param>
        /// <returns>The created message.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<MessageDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageRequest request)
        {
            var command = new SendMessageCommand(request, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetMessageById), new { messageId = result.Data!.Id }, result);
        }

        /// <summary>
        /// React to a chat message (e.g., emoji reaction).
        /// </summary>
        /// <param name="request">The reaction request.</param>
        /// <returns>Confirmation string.</returns>
        [HttpPost("react")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReactToMessage([FromBody] ReactToMessageRequest request)
        {
            var command = new ReactToMessageCommand(request);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Mark a message as read.
        /// </summary>
        /// <param name="messageId">The ID of the message to mark as read.</param>
        /// <returns>Confirmation string.</returns>
        [HttpPatch("{messageId}/read")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsRead(Guid messageId)
        {
            var command = new MarkAsReadCommand(messageId);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }

}
