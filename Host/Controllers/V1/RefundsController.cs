using Application.Dto;
using Application.Pagination;
using Application.Queries.Refunds.GetAll;
using Application.Queries.Refunds.GetById;
using Application.Queries.Refunds.GetBySalesOrderId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RefundsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RefundsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all refunds (paginated). Only accessible by Admin or Manager.
        /// </summary>
        /// <param name="query">Pagination request</param>
        /// <returns>Paginated list of RefundDto</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(PaginatedList<RefundDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll([FromQuery] PageRequest query)
        {
            var result = await _mediator.Send(new GetAllRefundsQuery(query));
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
        }

        /// <summary>
        /// Get refund by its ID
        /// </summary>
        /// <param name="refundId">Refund ID</param>
        /// <returns>RefundDto</returns>
        [HttpGet("{refundId}")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(RefundDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(Guid refundId)
        {
            var result = await _mediator.Send(new GetRefundByIdQuery(refundId));
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.Message);
        }

        /// <summary>
        /// Get refunds for a specific sales order
        /// </summary>
        /// <param name="orderId">Sales order ID</param>
        /// <returns>List of RefundDto</returns>
        [HttpGet("by-order/{orderId}")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<RefundDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByOrderId(Guid orderId)
        {
            var result = await _mediator.Send(new GetRefundBySalesOrderIdQuery(orderId));
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.Message);
        }
    }

}
