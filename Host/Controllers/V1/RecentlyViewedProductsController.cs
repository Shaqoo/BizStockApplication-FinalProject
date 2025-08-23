using Application.Commands.RecentlyCheckedProduct.AddRecentlyViewedProduct;
using Application.Commands.RecentlyCheckedProduct.ClearRecentlyViewedProducts;
using Application.Commands.RecentlyCheckedProduct.RemoveRecentlyViewedProduct;
using Application.Dto;
using Application.Extensions;
using Application.Queries.RecentlyCheckedProducts.GetRecentlyViewedProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Host.Controllers.V1
{
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]

    public class RecentlyViewedProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RecentlyViewedProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add a product to the recently viewed list.
        /// </summary>
        /// <param name="productRequest">The product to add to recently viewed.</param>
        /// <returns>Result of the add operation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRecentlyViewedProducts([FromBody] AddRecentlyViewedProductRequest productRequest)
        {
            var command = new AddRecentlyViewedProductCommand(productRequest);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Clear all recently viewed products for the current session.
        /// </summary>
        /// <returns>Result of the clear operation.</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClearRecentlyViewedProducts()
        {
            var sessionId = HttpContext.Request.Cookies[RecentlyViewedProductSessionExtension.RecentlyViewedProductSessionKey];

            var command = new ClearRecentlyViewedProductsCommand(sessionId);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Remove a specific product from recently viewed by productId.
        /// </summary>
        /// <param name="productId">The ID of the product to remove.</param>
        /// <returns>Result of the remove operation.</returns>
        [HttpDelete("by-productId/{productId:guid}")]
        [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveRecentlyViewedProduct([FromRoute] Guid productId)
        {
            var sessionId = HttpContext.Request.Cookies[RecentlyViewedProductSessionExtension.RecentlyViewedProductSessionKey];

            var command = new RemoveRecentlyViewedProductCommand(sessionId, productId);
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Get the list of recently viewed products for the logged-in user or guest session.
        /// </summary>
        /// <returns>A list of recently viewed products.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<RecentlyViewedProductDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRecentlyViewedProducts()
        {
            GetRecentlyViewedProductsQuery? query = null;

            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdClaim, out Guid userId))
                {
                    query = new(userId, null);
                }
                else
                {
                    return BadRequest("Invalid user identifier.");
                }
            }
            else
            {
                var sessionId = HttpContext.GetOrAddRecentlyViewedProductSession();
                if (string.IsNullOrEmpty(sessionId))
                    return BadRequest("No valid user or session found.");

                query = new(Guid.Empty, sessionId);
            }

            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
