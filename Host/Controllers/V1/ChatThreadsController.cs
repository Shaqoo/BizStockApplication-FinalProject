using Application.Commands.ChatThreads.Assign;
using Application.Commands.ChatThreads.Close;
using Application.Commands.ChatThreads.Create;
using Application.Dto;
using Application.Pagination;
using Application.Queries.ChatThreads.GetAllChatThreads;
using Application.Queries.ChatThreads.GetById;
using Application.Queries.ChatThreads.GetByStatus;
using Application.Queries.ChatThreads.GetChatThreadsByAgent;
using Application.Queries.ChatThreads.GetChatThreadsByCustomer;
using Application.Queries.ChatThreads.GetCustomerOpenThread;
using Application.Queries.ChatThreads.GetStats;
using Domain.Enums;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]  
    public class ChatThreadsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatThreadsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the chat thread statistics for the currently logged-in CSO.
        /// </summary>
        /// <returns>Number of open, in-progress, closed, and total chat threads.</returns>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(Result<ChatThreadStatsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetChatThreadStats()
        {
            var result = await _mediator.Send(new GetChatThreadStatsQuery());
            if (!result.IsSuccess)
                return StatusCode(500, result);

            return Ok(result);
        }


        /// <summary>
        /// Creates a new chat thread for the current user.
        /// </summary>
        /// <param name="command">The create chat thread command.</param>
        /// <returns>Thread Id of the newly created chat thread.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "Customer,CustomerService")]
        public async Task<IActionResult> CreateChatThread()
        {
            var result = await _mediator.Send(new CreateChatThreadCommand(Request.GetRequestMetadata()));
            return Ok(result);
        }

        /// <summary>
        /// Closes an existing chat thread.
        /// </summary>
        /// <param name="threadId">Chat thread Id to close.</param>
        /// <param >Metadata about the request.</param>
        [HttpPut("{threadId}/close")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "CustomerService,Admin")]
        public async Task<IActionResult> CloseChatThread(Guid threadId)
        {
            var result = await _mediator.Send(new CloseChatThreadCommand(threadId, Request.GetRequestMetadata()));
            return Ok(result);
        }

        /// <summary>
        /// Assigns an agent to an existing chat thread.
        /// </summary>
        /// <param name="threadId">The chat thread Id.</param>
        /// <param name="agentId">The agent Id.</param>
        /// <param >Metadata about the request.</param>
        [HttpPut("{threadId}/assign/{agentId}")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "Manager,Admin,CustomerService")]
        public async Task<IActionResult> AssignAgentToThread(Guid threadId, Guid agentId)
        {
            var result = await _mediator.Send(new AssignAgentToThreadCommand(threadId, agentId, Request.GetRequestMetadata()));
            return Ok(result);
        }

        /// <summary>
        /// Gets a paginated list of all chat threads.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedList<ChatThreadDto>>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "Admin,Manager,CustomerServiceOfficer")]
        public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetAllChatThreadsQuery(pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Gets a chat thread by Id.
        /// </summary>
        [HttpGet("{threadId}")]
        [ProducesResponseType(typeof(Result<ChatThreadDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(Guid threadId)
        {
            var result = await _mediator.Send(new GetChatThreadByIdQuery(threadId));
            return Ok(result);
        }

        /// <summary>
        /// Gets chat threads filtered by status.
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(Result<PaginatedList<ChatThreadDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByStatus(ChatStatus status, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetChatThreadsByStatusQuery(status, pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Gets chat threads assigned to the currently logged-in agent.
        /// </summary>
        [HttpGet("assigned")]
        [ProducesResponseType(typeof(Result<PaginatedList<ChatThreadDto>>), (int)HttpStatusCode.OK)]
        [Authorize(Roles = "CustomerService")]
        public async Task<IActionResult> GetByAssignedAgent([FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetChatThreadsByAssignedAgentQuery(pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Gets chat threads created by the current user.
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(Result<PaginatedList<ChatThreadDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByCurrentUser([FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetChatThreadsByCurrentUserQuery(pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Gets open chat thread created by the current user.
        /// </summary>
        [HttpGet("my-open-thread")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(Result<PaginatedList<ChatThreadDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerOpenThread()
        {
            var result = await _mediator.Send(new GetCustomerOpenThreadQuery());
            return result.ToActionResult(this);
        }
    }

}
