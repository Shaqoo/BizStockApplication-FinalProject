using Application.Queries.SalesOrders.GetOrderCostAndETA;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiversion}/[controller]")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SalesOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get estimated order cost and delivery time for a given delivery address.
        /// </summary>
        [HttpGet("{deliveryAddressId:guid}/cost-eta")]
        public async Task<IActionResult> GetOrderCostAndETA(Guid deliveryAddressId)
        {
            var query = new GetOrderCostAndETAQuery(deliveryAddressId, Request.GetRequestMetadata());

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
