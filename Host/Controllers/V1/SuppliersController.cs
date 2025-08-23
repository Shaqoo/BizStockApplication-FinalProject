using Application.Commands.Suppliers.Create;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Host.Controllers.V1
{
    /// <summary>
    /// Provides endpoints for managing supplier-related operations, such as creating new suppliers.
    /// </summary>
    /// <remarks>This controller is part of API version 1.0 and is accessible via the route pattern
    /// "api/v{version}/suppliers". It uses the mediator pattern to handle requests and responses.</remarks>
    /// <param name="mediator"></param>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class SuppliersController(IMediator mediator) : ControllerBase
    {

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateSupplierCommand(request, Request.GetRequestMetadata());
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(CreateSupplier), new { id = result.Data }, result);
        }


    }
}
