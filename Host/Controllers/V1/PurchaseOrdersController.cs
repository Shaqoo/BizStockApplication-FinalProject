using Application.Commands.PurchaseOrders.AddPurchaseOrderItem;
using Application.Commands.PurchaseOrders.CreatePurchaseOrder;
using Application.Commands.PurchaseOrders.RemovePurchaseOrderItem;
using Application.Commands.PurchaseOrders.UpdatePurchaseOrder;
using Application.Commands.PurchaseOrders.UpdatePurchaseOrderItem;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

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
        /// <summary>
        /// Update an existing purchase order.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase order.</param>
        /// <param name="dto">The <c>UpdatePurchaseOrderCommand</c> containing updated details.</param>
        /// <returns>A <c>Result&lt;Guid&gt;</c> indicating the updated purchase order ID.</returns>
        /// <response code="200">Successfully updated the purchase order.</response>
        /// <response code="400">Validation failed or request was invalid.</response>
        /// <response code="404">Purchase order not found.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePurchaseOrderDto dto)
        {
            if (id != dto.PurchaseOrderId)
                return BadRequest("ID in URL does not match command ID.");

            var result = await _mediator.Send(new UpdatePurchaseOrderCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Add a new item to an existing purchase order.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase order.</param>
        /// <param name="dto">The <c>AddPurchaseOrderItemCommand</c> containing item details to add.</param>
        /// <returns>A <c>Result&lt;Guid&gt;</c> indicating the purchase order ID the item was added to.</returns>
        /// <response code="200">Item successfully added to the purchase order.</response>
        /// <response code="400">Validation failed or request was invalid.</response>
        /// <response code="404">Purchase order not found.</response>
        [HttpPost("{id:guid}/items")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddItem(Guid id, [FromBody] AddPurchaseOrderItemDto dto)
        {
            if (id != dto.PurchaseOrderId)
                return BadRequest("ID in URL does not match command ID.");

            var result = await _mediator.Send(new AddPurchaseOrderItemCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Update an item in a purchase order.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase order.</param>
        /// <param name="itemId">The unique identifier of the item to update.</param>
        /// <param name="dto">The <c>UpdatePurchaseOrderItemCommand</c> containing updated item details.</param>
        /// <returns>A <c>Result&lt;Guid&gt;</c> indicating the purchase order ID the item belongs to.</returns>
        /// <response code="200">Item successfully updated in the purchase order.</response>
        /// <response code="400">Validation failed or request was invalid.</response>
        /// <response code="404">Purchase order or item not found.</response>
        [HttpPut("{id:guid}/items/{itemId:guid}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItem(Guid id, Guid itemId, [FromBody] UpdatePurchaseOrderItemDto dto)
        {
            if (id != dto.PurchaseOrderId || itemId != dto.PurchaseOrderItemId)
                return BadRequest("ID in URL does not match command ID.");

            var result = await _mediator.Send(new UpdatePurchaseOrderItemCommand(dto,Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Remove an item from a purchase order.
        /// </summary>
        /// <param name="id">The unique identifier of the purchase order.</param>
        /// <param>The unique identifier of the item to remove.</param>
        /// <param name="dto">The <c>RemovePurchaseOrderItemCommand</c> containing the item removal request.</param>
        /// <returns>A <c>Result&lt;Guid&gt;</c> indicating the purchase order ID the item was removed from.</returns>
        /// <response code="200">Item successfully removed from the purchase order.</response>
        /// <response code="400">Validation failed or request was invalid.</response>
        /// <response code="404">Purchase order or item not found.</response>
        [HttpDelete("{id:guid}/items")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItem(Guid id,[FromBody] RemovePurchaseOrderItemDto dto)
        {
            if (id != dto.PurchaseOrderId)
                return BadRequest("ID in URL does not match command ID.");

            var result = await _mediator.Send(new RemovePurchaseOrderItemCommand(dto,Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}

