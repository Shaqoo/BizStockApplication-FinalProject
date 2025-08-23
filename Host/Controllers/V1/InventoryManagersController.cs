using Application.Commands.InventoryManagers.Create;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Host.Controllers.V1
{
    /// <summary>
    /// Provides API endpoints for managing inventory managers.
    /// </summary>
    /// <remarks>This controller is responsible for handling operations related to inventory managers, such as
    /// creating new inventory manager records. It uses the mediator pattern to delegate business logic to the
    /// appropriate handlers.</remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InventoryManagersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryManagersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new inventory manager with the provided details.
        /// </summary>
        /// <param name="request">The request model containing the details of the inventory manager to create.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the operation.
        /// Returns <see cref="BadRequestObjectResult"/> if the request model is invalid.
        /// Returns <see cref="CreatedAtActionResult"/> containing a <see cref="TwoFactorSetupDto"/> if successful.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(TwoFactorSetupDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateInventoryManager([FromBody] CreateInventoryManagerRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateInventoryManagerCommand(request, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(
                nameof(CreateInventoryManager),
                new { id = result.Data },  
                result.Data
            );
        }
    }

}
