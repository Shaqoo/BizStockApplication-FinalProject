using Application.Commands.Notifications.MarkAllAsRead;
using Application.Commands.Notifications.MarkAsRead;
using Application.Commands.Notifications.SendNotificationToUser;
using Application.Commands.Notifications.SendNotificationViaRoles;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.Notifications.CountUnreadByRecipient;
using Application.Queries.Notifications.GetById;
using Application.Queries.Notifications.GetNotificationsByRecipient;
using Application.Queries.Notifications.GetUnreadNotifications;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Send a notification to a specific user.
        /// </summary>
        [HttpPost("send/user")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendToUser([FromBody] NotificationRequest request)
        {
            var command = new SendNotificationToUserCommand(request);
            var result = await _mediator.Send(command);
            return result.IsSuccess? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Send a notification to all users in a role.
        /// </summary>
        [HttpPost("send/role")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendToRole([FromBody] NotificationRequest request)
        {
            var command = new SendNotificationToRoleCommand(request);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Mark a notification as read by Id.
        /// </summary>
        [HttpPut("mark-as-read/{notificationId:guid}")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var result = await _mediator.Send(new MarkNotificationAsReadCommand(notificationId));
            return Ok(result);
        }

        /// <summary>
        /// Mark all notifications as read for a recipient.
        /// </summary>
        [HttpPut("mark-all-as-read")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var result = await _mediator.Send(new MarkAllNotificationsAsReadCommand());
            return Ok(result);
        }

        /// <summary>
        /// Get a notification by Id.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetNotificationByIdQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Get all unread notifications for a recipient.
        /// </summary>
        [HttpGet("recipient/unread")]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnreadByRecipient()
        {
            var result = await _mediator.Send(new GetUnreadNotificationsByRecipientQuery());
            return Ok(result);
        }

        /// <summary>
        /// Count unread notifications for a recipient.
        /// </summary>
        [HttpGet("recipient/count-unread")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<IActionResult> CountUnreadByRecipient()
        {
            var result = await _mediator.Send(new CountUnreadByRecipientQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get paginated notifications for a recipient.
        /// </summary>
        [HttpGet("recipient/paged")]
        [ProducesResponseType(typeof(PaginatedList<Notification>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByRecipientPaged([FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetNotificationsByRecipientPagedQuery(pageRequest));
            return Ok(result);
        }
    }

}
