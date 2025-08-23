using Application.Commands.Carts.AddItem;
using Application.Commands.Carts.Create;
using Application.Commands.Carts.LinkToUser;
using Application.Commands.Carts.RemoveItem;
using Application.Commands.Carts.Update;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.Carts.GetById;
using Application.Queries.Carts.GetBySessionId;
using Application.Queries.Carts.GetByUserId;
using Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CartsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new shopping <c>Cart</c>.
        /// </summary>
        /// <param name="request">The <c>CreateCartRequest</c> containing session or user information.</param>
        /// <returns>The created <c>CartDto</c>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<CartDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<CartDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request)
        {
            var result = await _mediator.Send(new CreateCartCommand(request));
            return StatusCode(result.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Adds an item to the <c>Cart</c>.
        /// </summary>
        /// <param name="request">The <c>AddCartItemRequest</c> containing product details.</param>
        /// <returns>The added <c>CartItemDto</c>.</returns>
        [HttpPost("items")]
        [ProducesResponseType(typeof(Result<CartItemDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<CartItemDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemRequest request)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                request.SetCartSessionId(HttpContext.GetOrCreateCartSessionId());
            }
            else
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdClaim, out var userId))
                {
                    request.UserId = userId;
                }
                else
                {
                    return BadRequest(Result<CartItemDto>.Failure("Invalid user ID in claims"));
                }
            }
            var result = await _mediator.Send(new AddCartItemCommand(request));

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }


        /// <summary>
        /// Updates the quantity of a <c>CartItem</c>.
        /// </summary>
        /// <param name="request">The <c>UpdateCartItemQuantityRequest</c> containing new quantity.</param>
        /// <returns>Status message.</returns>
        [HttpPut("items")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartItemQuantityRequest request)
        {
            var result = await _mediator.Send(new UpdateCartItemQuantityCommand(request));
            return StatusCode(result.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Removes a <c>CartItem</c> from the <c>Cart</c>.
        /// </summary>
        /// <param name="request">The <c>RemoveCartItemRequest</c> containing the item identifier.</param>
        /// <returns>Status message.</returns>
        [HttpDelete("items")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveCartItem([FromQuery] RemoveCartItemRequest request)
        {
            var result = await _mediator.Send(new RemoveCartItemCommand(request));
            return StatusCode(result.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Links a guest <c>Cart</c> to a registered <c>User</c>.
        /// </summary>
        /// <param >The cart identifier.</param>
        /// <param >The user identifier.</param>
        /// <returns>Status message.</returns>
        [Authorize]
        [HttpPut("/link-cart-to-user")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LinkCartToUser()
        {
            var sessionId = HttpContext.GetOrCreateCartSessionId();
        
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                var result = await _mediator.Send(new LinkCartToUserCommand(userId, sessionId));
                return StatusCode(result.IsSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest, result);
            }
            else
            {
                return BadRequest(Result<CartItemDto>.Failure("Invalid user ID in claims"));
            }
        }

        /// <summary>
        /// Gets paginated <c>CartItems</c> by <c>CartId</c>.
        /// </summary>
        /// <param name="cartId">The cart identifier.</param>
        /// <param name="pageRequest">The pagination request (page number and size).</param>
        /// <returns>Paginated list of <c>CartItemDto</c>.</returns>
        [HttpGet("{cartId:guid}/items")]
        [ProducesResponseType(typeof(Result<PaginatedList<CartItemDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCartItems(Guid cartId, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetCartItemsQuery(cartId, pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Gets paginated <c>CartItems</c> by <c>UserId</c>.
        /// </summary>
        /// <param >The user identifier.</param>
        /// <param name="pageRequest">The pagination request.</param>
        /// <returns>Paginated list of <c>CartItemDto</c>.</returns>
        [Authorize]
        [HttpGet("user/items")]
        [ProducesResponseType(typeof(Result<PaginatedList<CartItemDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCartItemsByUserId([FromQuery] PageRequest pageRequest)
        {
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                var result = await _mediator.Send(new GetCartItemsByUserIdQuery(userId, pageRequest));
                return !result.IsSuccess ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest(Result<CartItemDto>.Failure("Invalid user ID in claims"));
            }
        }

        /// <summary>
        /// Gets paginated <c>CartItems</c> by <c>SessionId</c>.
        /// </summary>
        /// <param >The session identifier.</param>
        /// <param name="pageRequest">The pagination request.</param>
        /// <returns>Paginated list of <c>CartItemDto</c>.</returns>
        [HttpGet("session/items")]
        [ProducesResponseType(typeof(Result<PaginatedList<CartItemDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCartItemsBySessionId([FromQuery] PageRequest pageRequest)
        {
            var sessionId = HttpContext.GetOrCreateCartSessionId();
            var result = await _mediator.Send(new GetCartItemsBySessionIdQuery(sessionId, pageRequest));
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }
    }

}
