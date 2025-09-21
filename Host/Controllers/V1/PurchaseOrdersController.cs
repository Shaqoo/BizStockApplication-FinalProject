using Application.Commands.PurchaseOrders.AddPurchaseOrderItem;
using Application.Commands.PurchaseOrders.CancelPurchaseOrder;
using Application.Commands.PurchaseOrders.ConfirmPurchaseOrder;
using Application.Commands.PurchaseOrders.CreatePurchaseOrder;
using Application.Commands.PurchaseOrders.ReceivePurchaseOrderItems;
using Application.Commands.PurchaseOrders.RejectPurchaseOrder;
using Application.Commands.PurchaseOrders.RemovePurchaseOrderItem;
using Application.Commands.PurchaseOrders.UpdatePurchaseOrder;
using Application.Commands.PurchaseOrders.UpdatePurchaseOrderItem;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.PurchaseOrders.GetPurchaseOrderById;
using Application.Queries.PurchaseOrders.GetPurchaseOrderList;
using Application.Queries.PurchaseOrders.GetPurchaseOrdersByDateRange;
using Application.Queries.PurchaseOrders.GetPurchaseOrdersByStatus;
using Application.Queries.PurchaseOrders.GetPurchaseOrdersBySupplier;
using Application.Queries.PurchaseOrders.GetPurchaseOrderStats;
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

        /// <summary>
        /// Cancels a purchase order if it has not already been received.
        /// </summary>
        /// <param name="purchaseOrderId">The unique identifier of the purchase order to cancel.</param>
        /// <param name="dto">The cancellation details, including reason.</param>
        /// <returns>
        /// A <see cref="Result{Boolean}"/> indicating whether the purchase order was successfully cancelled.
        /// </returns>
        /// <remarks>
        /// Only purchase orders with status <c>Draft</c>, <c>Confirmed</c>, or <c>PartiallyReceived</c> 
        /// can be cancelled. Orders that are <c>Received</c> or already <c>Cancelled</c> 
        /// cannot be cancelled.
        /// </remarks>
        [HttpPatch("{purchaseOrderId:guid}/cancel")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelPurchaseOrder(Guid purchaseOrderId, [FromBody] CancelPurchaseOrderDto dto)
        {
            var command = new CancelPurchaseOrderCommand(purchaseOrderId,dto,Request.GetRequestMetadata());

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Confirms an existing purchase order.
        /// </summary>
        /// <param name="purchaseOrderId">The unique identifier of the purchase order to confirm.</param>
        /// <param name="confirmDto">The confirmation details, including comments if any.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// Returns <c>true</c> if the purchase order was successfully confirmed; otherwise, <c>false</c>.
        /// </returns>
        [HttpPatch("{purchaseOrderId:guid}/confirm")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmPurchaseOrder(Guid purchaseOrderId,[FromBody] ConfirmPurchaseOrderDto confirmDto,
            CancellationToken cancellationToken)
        {
            var command = new ConfirmPurchaseOrderCommand(purchaseOrderId, confirmDto, Request.GetRequestMetadata());

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Rejects an existing purchase order.
        /// </summary>
        /// <param name="purchaseOrderId">The unique identifier of the purchase order to reject.</param>
        /// <param name="rejectDto">The rejection details, including reason.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// Returns <c>true</c> if the purchase order was successfully rejected; otherwise, <c>false</c>.
        /// </returns>
        [HttpPatch("{purchaseOrderId:guid}/reject")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectPurchaseOrder(Guid purchaseOrderId,[FromBody] RejectPurchaseOrderDto rejectDto,
            CancellationToken cancellationToken)
        {
            var command = new RejectPurchaseOrderCommand(purchaseOrderId,rejectDto,Request.GetRequestMetadata());

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }


        /// <summary>
        /// Records the receipt of items for a specific purchase order.
        /// </summary>
        /// <param name="purchaseOrderId">The unique identifier of the purchase order.</param>
        /// <param name="warehouseId">The unique identifier of the warehouse where items are received.</param>
        /// <param name="items">The list of items being received, with <c>PurchaseOrderItemId</c> and <c>QuantityReceived</c>.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> with a boolean value indicating whether the operation succeeded.
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> if items were successfully received.</description></item>
        /// <item><description><c>400 BadRequest</c> if the request is invalid (e.g., quantity mismatch).</description></item>
        /// <item><description><c>404 NotFound</c> if the purchase order does not exist.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">If the items were successfully recorded as received.</response>
        /// <response code="400">If the request is invalid or missing required information.</response>
        /// <response code="404">If the purchase order was not found.</response>
        [HttpPost("{purchaseOrderId:guid}/receive-items")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReceiveItems(Guid purchaseOrderId,Guid warehouseId,[FromBody] List<ReceivePurchaseOrderItemDto> items)
        {
            var command = new ReceivePurchaseOrderItemsCommand(purchaseOrderId,warehouseId,items,Request.GetRequestMetadata());

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Get a paginated list of purchase orders.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<PurchaseOrderListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPurchaseOrders([FromQuery] PageRequest pageRequest)
        {
            var query = new GetPurchaseOrderListQuery(pageRequest);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get details of a purchase order by its ID.
        /// </summary>
        [HttpGet("{purchaseOrderId:guid}/get-byId")]
        [ProducesResponseType(typeof(Result<PurchaseOrderDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<PurchaseOrderDetailDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPurchaseOrderById(Guid purchaseOrderId)
        {
            var query = new GetPurchaseOrderByIdQuery(purchaseOrderId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Get a paginated list of purchase orders for a given supplier.
        /// </summary>
        [HttpGet("supplier/{supplierId:guid}")]
        [ProducesResponseType(typeof(PaginatedList<PurchaseOrderListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPurchaseOrdersBySupplier(Guid supplierId, [FromQuery] PageRequest pageRequest)
        {
            var query = new GetPurchaseOrdersBySupplierQuery(supplierId, pageRequest);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a paginated list of purchase orders filtered by status.
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(PaginatedList<PurchaseOrderListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPurchaseOrdersByStatus(PurchaseOrderStatus status, [FromQuery] PageRequest pageRequest)
        {
            var query = new GetPurchaseOrdersByStatusQuery(status, pageRequest);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get overall purchase order statistics (counts, totals, etc.).
        /// </summary>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(Result<PurchaseOrderStatsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPurchaseOrderStats()
        {
            var query = new GetPurchaseOrderStatsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a paginated list of purchase orders filtered by DateRange.
        /// </summary>
        [HttpGet("date-range")]
        [ProducesResponseType(typeof(PaginatedList<PurchaseOrderListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPurchaseOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery]DateTime endDate, [FromQuery] PageRequest pageRequest)
        {
            var query = new GetPurchaseOrdersByDateRangeQuery(startDate,endDate, pageRequest);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}



