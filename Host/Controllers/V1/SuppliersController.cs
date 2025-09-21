using Application.Commands.Suppliers.Create;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.Suppliers;
using Application.Queries.Suppliers.GetAllSuppliers;
using Application.Queries.Suppliers.GetByEmail;
using Host.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using IMediator = MediatR.IMediator;

namespace Host.Controllers.V1
{
    /// <summary>
    /// Provides endpoints for managing supplier-related operations, such as creating new suppliers.
    /// </summary>
    /// <remarks>This controller is part of API version 1.0 and is accessible via the route pattern
    /// "api/v{version}/suppliers". It uses the mediator pattern to handle requests and responses.</remarks>
    /// <param ></param>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class SuppliersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SuppliersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new supplier based on the provided request data.
        /// </summary>
        /// <param name="request">The request model containing the details of the supplier to be created.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the operation.
        /// Returns <see cref="BadRequestObjectResult"/> if the request model is invalid.
        /// Returns <see cref="CreatedAtActionResult"/> with the two-factor setup details if the operation is successful.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<TwoFactorSetupDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<TwoFactorSetupDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequestModel request)
        {

            var command = new CreateSupplierCommand(request, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(CreateSupplier), new { id = result.Data }, result);
        }


        /// <summary>
        /// View the details of a supplier.
        /// </summary>
        /// <returns>Supplier details</returns>
        [Authorize]
        [HttpGet("supplier-me")]
        [ProducesResponseType(typeof(Result<SupplierDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ViewSupplierDetails()
        {
            var query = new ViewSupplierDetailsQuery(); 
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a supplier by their email address.
        /// </summary>
        /// <param name="email">The email of the supplier.</param>
        /// <returns>The supplier details.</returns>
        [HttpGet("by-email/{email}")]
        [ProducesResponseType(typeof(SupplierDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var query = new GetSuppliersByEmailQuery(email);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess || result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Gets a paginated list of all suppliers.
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns>A Paged Data Of Suppliers</returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<SupplierDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSuppliers([FromQuery] PageRequest pageRequest)
        {
            var query = new GetAllSuppliersQuery(pageRequest);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
