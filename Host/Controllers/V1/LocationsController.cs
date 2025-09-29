using Application.Queries.Lgas.GetLgaById;
using Application.Queries.Lgas.GetLgasByStateId;
using Application.Queries.States.GetAllStates;
using Application.Queries.States.GetStateById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LocationsController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Get all states (with their LGAs).
        /// </summary>
        [HttpGet("states")]
        public async Task<IActionResult> GetAllStates()
        {
            var states = await _mediator.Send(new GetAllStatesQuery());
            return Ok(states);
        }

        /// <summary>
        /// Get a state by its ID.
        /// </summary>
        /// <param name="id">The ID of the state.</param>
        [HttpGet("states/{id:int}")]
        public async Task<IActionResult> GetStateById(int id)
        {
            var state = await _mediator.Send(new GetStateByIdQuery(id));
            if (state == null) return NotFound();
            return Ok(state);
        }

        /// <summary>
        /// Get all LGAs in a specific state.
        /// </summary>
        /// <param name="stateId">The ID of the state.</param>
        [HttpGet("states/{stateId:int}/lgas")]
        public async Task<IActionResult> GetLgasByStateId(int stateId)
        {
            var lgas = await _mediator.Send(new GetLgasByStateIdQuery(stateId));
            return Ok(lgas);
        }

        /// <summary>
        /// Get an LGA by its ID.
        /// </summary>
        /// <param name="id">The ID of the LGA.</param>
        [HttpGet("lgas/{id:int}")]
        public async Task<IActionResult> GetLgaById(int id)
        {
            var lga = await _mediator.Send(new GetLgaByIdQuery(id));
            if (lga == null) return NotFound();
            return Ok(lga);
        }
    }

}
