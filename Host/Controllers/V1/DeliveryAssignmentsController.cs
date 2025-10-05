using Application.Dto;
using Application.Pagination;
using Application.Queries.DeliveryAssignments.GetAll;
using Application.Queries.DeliveryAssignments.GetById;
using Application.Queries.DeliveryAssignments.GetBySalesOrderId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class DeliveryAssignmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryAssignmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all delivery assignments (Admin/Logistics only).
        /// </summary>
        /// <param name="pageRequest">Pagination request (page number, page size)</param>
        /// <returns>Paginated list of delivery assignments</returns>
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedList<DeliveryAssignmentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetDeliveryAssignmentsQuery(pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Get delivery assignment by its unique identifier.
        /// </summary>
        /// <param name="id">Delivery assignment ID</param>
        /// <returns>Delivery assignment details</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<DeliveryAssignmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetDeliveryAssignmentByIdQuery(id));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Get delivery assignment for a specific sales order.
        /// </summary>
        /// <param name="orderId">Sales order ID</param>
        /// <returns>Delivery assignment linked to the sales order</returns>
        [HttpGet("by-order/{orderId:guid}")]
        [ProducesResponseType(typeof(Result<DeliveryAssignmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByOrderId(Guid orderId)
        {
            var result = await _mediator.Send(new GetDeliveryAssignmentByOrderIdQuery(orderId));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }
    }

}
