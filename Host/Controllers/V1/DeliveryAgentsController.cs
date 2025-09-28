using Application.Commands.DeliveryAgents.Create;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Queries.DeliveryAgents.GetByEmail;
using Application.Queries.DeliveryAgents.ViewMyDetails;
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

    public class DeliveryAgentsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Creates a new delivery agent.
        /// </summary>
        /// <param name="request">The delivery agent creation details.</param>
        /// <returns>The newly created delivery agent's details.</returns>
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ProducesResponseType(typeof(DeliveryAgentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryAgentModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await mediator.Send(new CreateDeliveryAgentCommand(request,Request.GetRequestMetadata()));

            return CreatedAtAction(nameof(Create), new { id = result.Data});
        }


        /// <summary>
        /// View the details of a delivery agent.
        /// </summary>
        /// <returns>Delivery agent details</returns>
        [HttpGet("delivery-agent-me")]
        [ProducesResponseType(typeof(Result<DeliveryAgentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ViewDeliveryAgent()
        {
            var query = new ViewDeliveryAgentQuery();  
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a delivery agent by their email address.
        /// </summary>
        /// <param name="email">The email of the delivery agent.</param>
        /// <returns>The delivery agent details.</returns>
        [HttpGet("by-email/{email}")]
        [ProducesResponseType(typeof(DeliveryAgentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var query = new GetDeliveryAgentEmailQuery(email);
            var result = await mediator.Send(query);

            if (!result.IsSuccess || result.Data == null)
                return NotFound(result);

            return Ok(result);
        }
    }
}
