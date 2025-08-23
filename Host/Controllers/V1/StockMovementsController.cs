using Application.Commands.StockMovements.AdjustInventory;
using Application.Commands.StockMovements.TransferStock;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.StockMovements.GetAll;
using Application.Queries.StockMovements.GetByDateRange;
using Application.Queries.StockMovements.GetById;
using Application.Queries.StockMovements.GetByMovementType;
using Application.Queries.StockMovements.GetByProduct;
using Application.Queries.StockMovements.GetByWarehouse;
using Domain.Enums;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StockMovementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockMovementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves paginated stock movements for a specific warehouse.
        /// </summary>
        /// <param name="warehouseId">The unique identifier of the warehouse.</param>
        /// <param name="pageRequest">Pagination parameters (page number, page size).</param>
        /// <returns>A paginated list of stock movements.</returns>
        [Authorize]
        [HttpGet("warehouse/{warehouseId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<StockMovementDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockMovementsByWarehouseId([FromRoute] Guid warehouseId, [FromQuery] PageRequest pageRequest)
        {
            var query = new GetStockMovementsByWarehouseIdQuery(warehouseId, pageRequest);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves paginated stock movements for a specific product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="pageRequest">Pagination parameters (page number, page size).</param>
        /// <returns>A paginated list of stock movements.</returns>
        [Authorize]
        [HttpGet("product/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<StockMovementDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockMovementsByProductId([FromRoute] Guid productId, [FromQuery] PageRequest pageRequest)
        {
            var query = new GetStockMovementByProductIdQuery(productId, pageRequest);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves paginated stock movements by movement type.
        /// </summary>
        /// <param name="movementType">The stock movement type (e.g., Transfer, Adjustment).</param>
        /// <param name="pageRequest">Pagination parameters.</param>
        /// <returns>A paginated list of stock movements.</returns>
        [Authorize]
        [HttpGet("type/{movementType}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<StockMovementDto>))]
        public async Task<IActionResult> GetStockMovementsByType([FromRoute] StockMovementType movementType, [FromQuery] PageRequest pageRequest)
        {
            var query = new GetStockMovementsByMovementTypeQuery(movementType, pageRequest);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
 
        /// <summary>
        /// Retrieves a specific stock movement by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the stock movement.</param>
        /// <returns>The stock movement details.</returns>
        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockMovementDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStockMovementById([FromRoute] Guid id)
        {
            var query = new GetStockMovementByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves paginated stock movements within a given date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <param name="pageRequest">Pagination parameters.</param>
        /// <returns>A paginated list of stock movements.</returns>
        [Authorize]
        [HttpGet("date-range")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<StockMovementDto>))]
        public async Task<IActionResult> GetStockMovementsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] PageRequest pageRequest)
        {
            var query = new GetStockMovementsByDateRangeQuery(startDate, endDate, pageRequest);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all stock movements with pagination.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters.</param>
        /// <returns>A paginated list of all stock movements.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<StockMovementDto>))]
        public async Task<IActionResult> GetAllStockMovements([FromQuery] PageRequest pageRequest)
        {
            var query = new GetAllStockMovementsQuery(pageRequest);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Transfers stock between warehouses.
        /// </summary>
        /// <param name="request">The transfer stock request details.</param>
        /// <returns>A confirmation message.</returns>
        [Authorize]
        [HttpPost("transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TransferStock([FromBody] TransferStockRequest request)
        {
            var command = new TransferStockCommand(request, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Adjusts stock (increase/decrease) for a product in a warehouse.
        /// </summary>
        /// <param name="request">The stock adjustment request details.</param>
        /// <returns>A confirmation message.</returns>
        [Authorize]
        [HttpPost("adjust")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AdjustStock([FromBody] AdjustStockRequest request)
        {
            var command = new AdjustStockCommand(request, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
