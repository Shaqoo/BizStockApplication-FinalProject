using Application.Commands.CustomerService.Create;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Host.Controllers.V1
{
    /// <summary>
    /// Provides API endpoints for managing customer service officers.
    /// </summary>
    /// <remarks>This controller includes operations for creating and managing customer service officers. It
    /// is versioned as part of API version 1.0.</remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerServiceOfficersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerServiceOfficersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new customer service officer based on the provided request data.
        /// </summary>
        /// <param name="request">The request model containing the details of the officer to be created.</param>
        /// <returns>
        /// Returns a <see cref="TwoFactorSetupDto"/> if the officer is created successfully.
        /// </returns>
        /// <response code="201">Officer created successfully, returns TwoFactorSetupDto.</response>
        /// <response code="400">Invalid request model.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TwoFactorSetupDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOfficer([FromBody] CreateCustomerServiceRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateCustomerServiceCommand(request, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(CreateOfficer), new { id = result.Data }, result);
        }
    }

}
