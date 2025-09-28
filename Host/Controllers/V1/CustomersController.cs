using Application.Commands.Customers.Create;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Queries.Customers;
using Application.Queries.Customers.GetCustomerByEmail;
using Application.Queries.Customers.GetCustomerStats;
using Application.Queries.Suppliers.GetByEmail;
using Host.Extensions;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Host.Controllers.V1
{
    /// <summary>
    /// Provides API endpoints for managing customer-related operations.
    /// </summary>
    /// <remarks>This controller is versioned and accessible via routes that include the API version. For
    /// example: <c>api/v1.0/customers</c>.</remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class CustomersController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Creates a new customer based on the provided request data.
        /// </summary>
        /// <param name="request">The request model containing the details of the customer to be created.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the operation.
        /// Returns <see cref="BadRequestObjectResult"/> if the request model is invalid.
        /// Returns <see cref="CreatedAtActionResult"/> with a <see cref="TwoFactorSetupDto"/> 
        /// representing the two-factor setup information if the operation is successful.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<TwoFactorSetupDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result<TwoFactorSetupDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequestModel request)
        {

            var command = new CreateCustomerCommand(request, Request.GetRequestMetadata());
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(CreateCustomer), new { id = result.Data }, result);
        }

        /// <summary>
        /// Retrieves the details of the currently authenticated customer.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches customer information associated with the current logged-in user.
        /// </remarks>
        /// <response code="200">Returns the customer details as a <see cref="CustomerDto"/>.</response>
        /// <response code="400">If the request is invalid or the customer details could not be retrieved.</response>
        [Authorize]
        [HttpGet("current")]
        [ProducesResponseType(typeof(Result<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<CustomerDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrentCustomer()
        {
            var query = new ViewCustomerDetailsQuery();
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }


        /// <summary>
        /// Get customer statistics for customer service dashboard.
        /// </summary>
        /// <remarks>
        /// Returns total customers, verified customers, total orders, and open complaints.
        /// </remarks>
        /// <returns>A <see cref="CustomerStatsDto"/> object containing summary counts.</returns>
        /// <response code="200">Returns the customer statistics successfully.</response>
        [Authorize]
        [HttpGet("stats")]
        [ProducesResponseType(typeof(CustomerStatsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerStats()
        {
            var result = await mediator.Send(new GetCustomerStatsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a Customer by their email address.
        /// </summary>
        /// <param name="email">The email of the supplier.</param>
        /// <returns>The supplier details.</returns>
        [HttpGet("by-email/{email}")]
        [ProducesResponseType(typeof(CustomerDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var query = new GetCustomerByEmailQuery(email);
            var result = await mediator.Send(query);

            if (!result.IsSuccess || result.Data == null)
                return NotFound(result);

            return Ok(result);
        }

    }
}
