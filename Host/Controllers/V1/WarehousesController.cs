using Application.Commands.Warehouses.Activate;
using Application.Commands.Warehouses.Create;
using Application.Commands.Warehouses.Deactivate;
using Application.Commands.Warehouses.Update;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.Warehouses.GetAll;
using Application.Queries.Warehouses.GetById;
using Application.Queries.Warehouses.Search;
using Application.Queries.Warehouses.SearchProductStock;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehousesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new warehouse.
        /// </summary>
        /// <param name="warehouseDto">The warehouse details to create.</param>
        /// <returns>The created warehouse ID and status.</returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto warehouseDto)
        {
            var command = new CreateWarehouseCommand(warehouseDto, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess || result.Data is null)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetWarehouseById), new { id = result.Data.Id }, result.Data);
        }

        /// <summary>
        /// Updates an existing warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse to update.</param>
        /// <param name="warehouseDto">The updated warehouse details.</param>
        /// <returns>The update operation result.</returns>
        [Authorize]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWarehouse(Guid id, [FromBody] UpdateWarehouseDto warehouseDto)
        {
            var command = new UpdateWarehouseCommand(id, warehouseDto);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Deactivates a warehouse (soft delete).
        /// </summary>
        /// <param name="id">The ID of the warehouse to deactivate.</param>
        /// <returns>The deactivation operation result.</returns>
        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateWarehouse([FromRoute] Guid id)
        {
            var command = new DeactivateWarehouseCommand(id, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Activates a previously deactivated warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse to activate.</param>
        /// <returns>No content if successful.</returns>
        [Authorize]
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivateWarehouse([FromRoute] Guid id)
        {
            var command = new ActivateWarehouseCommand(id, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a warehouse by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the warehouse to retrieve.</param>
        /// <returns>The warehouse details.</returns>
        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WarehouseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWarehouseById([FromRoute] Guid id)
        {
            var query = new GetWarehouseByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound($"Warehouse with ID {id} not found");

            return Ok(result);
        }

        /// <summary>
        /// Retrieves all warehouses.
        /// </summary>
        ///<param name="pageRequest">The paging items such as the page size and number.</param>"
        /// <returns>A Paginated list of all warehouses.</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<WarehouseDto>))]
        public async Task<IActionResult> GetAllWarehouses([FromQuery]PageRequest pageRequest)
        {
            var query = new GetAllWarehousesQuery(pageRequest);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        
        /// <summary>
        /// Searches warehouses by a keyword (e.g., name, location).
        /// </summary>
        /// <param name="keyword">The keyword to search for.</param>
        ///<param name="pageRequest">The paging items such as the page size and number.</param>
        /// <returns>A Paginated list of matching warehouses.</returns>
        [Authorize]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<WarehouseDto>))]
        public async Task<IActionResult> SearchWarehouses([FromQuery] string keyword, [FromQuery] PageRequest pageRequest)
        {
            var query = new SearchWarehouseQuery(keyword,pageRequest);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Searches product stock across all warehouses by a keyword (e.g., product name, SKU).
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageRequest"></param>
        /// <returns>Paginated List Of ProductStockDto</returns>
        [HttpGet("search-productstocksummary")]
        [Authorize]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductStockSummaryDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductStockSummaryDto>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchProductStock([FromQuery] string keyword,[FromQuery]PageRequest pageRequest)
        {
            var result = await _mediator.Send(new SearchProductStockQuery(keyword,pageRequest));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

    }
}
