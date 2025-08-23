using Application.Commands.DeliveryAgents.Create;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
    }
}
