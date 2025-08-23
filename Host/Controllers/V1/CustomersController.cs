using Application.Commands.Customers.Create;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateCustomerCommand(request, Request.GetRequestMetadata());
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(CreateCustomer), new { id = result.Data }, result);
        }

    }
}
