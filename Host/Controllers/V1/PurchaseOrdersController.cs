using Application.Commands.PurchaseOrders.CreatePurchaseOrder;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Host.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new purchase order for a supplier with items.
        /// </summary>
        /// <param name="dto">The purchase order details including supplier, items, discount, tax, and expected delivery date.</param>
        /// <returns>
        /// A result containing the unique ID of the created purchase order if successful.
        /// </returns>
        /// <response code="201">Returns the created purchase order ID.</response>
        /// <response code="400">If validation fails or supplier/product does not exist.</response>
        /// <response code="401">If the user is unauthorized.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreatePurchaseOrderDto dto)
        {
            var command = new CreatePurchaseOrderCommand(dto, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        /// <summary>
        /// Gets a purchase order by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase order.</param>
        /// <returns>The purchase order details if found.</returns>
        /// <response code="200">Returns the purchase order details.</response>
        /// <response code="404">If the purchase order is not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            await Task.CompletedTask; 
            return Ok(new { Id = id, Message = "Purchase order details would be here." });
        }
    }
}
