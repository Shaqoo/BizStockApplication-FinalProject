using Application.Commands.Managers;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ManagersController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Creates a new manager in the system.
        /// </summary>
        /// <param name="request">The manager creation details.</param>
        /// <returns>The Setup For Two Factor</returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(TwoFactorSetupDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateManager([FromBody] CreateManagerRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateManagerCommand(request, Request.GetRequestMetadata());
            var result = await mediator.Send(command);

            return CreatedAtAction(nameof(CreateManager), result);
        }
    }

}
