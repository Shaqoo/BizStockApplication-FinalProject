using Application.Commands.Carts.AddItem;
using Application.Commands.Carts.ClearCart;
using Application.Commands.Carts.Create;
using Application.Commands.Carts.DecreaseCartItemQuantity;
using Application.Commands.Carts.LinkToUser;
using Application.Commands.Carts.RemoveItem;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Extensions;
using Application.Pagination;
using Application.Queries.Carts.GetByCurrentUser;
using Application.Queries.Carts.GetById;
using Application.Queries.Carts.GetBySessionId;
using Application.Queries.Carts.GetByUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// Get the current user's cart with paginated items.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <returns>Paginated cart DTO</returns>
        [Authorize]
        [HttpGet("current")]
        [ProducesResponseType(typeof(Result<PaginatedCartDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCurrentUserCart([FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetCartByCurrentUserQuery(pageRequest));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Get a cart by its ID with paginated items.
        /// </summary>
        /// <param name="cartId">Cart ID</param>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <returns>Paginated cart DTO</returns>
        [HttpGet("{cartId:guid}")]
        [ProducesResponseType(typeof(Result<PaginatedCartDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCartById(Guid cartId, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetCartItemsQuery(cartId, pageRequest));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Get a cart by session ID with paginated items.
        /// </summary>
        /// <param >Session ID</param>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <returns>Paginated cart DTO</returns>
        [HttpGet("session")]
        [ProducesResponseType(typeof(Result<PaginatedCartDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCartBySessionId([FromQuery] PageRequest pageRequest)
        {
            var sessionId = CartSessionExtension.GetOrCreateCartSessionId(HttpContext);
            var result = await _mediator.Send(new GetCartBySessionIdQuery(sessionId, pageRequest));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Get a cart by user ID with paginated items (admin or service use).
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <returns>Paginated cart DTO</returns>
        [HttpGet("user/{userId:guid}")]
        [ProducesResponseType(typeof(Result<PaginatedCartDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCartByUserId(Guid userId, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetCartByUserIdQuery(userId, pageRequest));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Create a new cart.
        /// </summary>
        /// <param name="request">CreateCartRequest</param>
        /// <returns>Created Cart DTO</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<CartDto>), 200)]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request)
        {
            var result = await _mediator.Send(new CreateCartCommand(request));
            if (!result.IsSuccess)
                return BadRequest(result); 
            return Ok(result);
        }

        /// <summary>
        /// Add an item to a cart.
        /// </summary>
        /// <param name="request">AddCartItemRequest</param>
        /// <returns>Updated Cart DTO</returns>
        [HttpPost("item")]
        [ProducesResponseType(typeof(Result<CartDto>), 200)]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemRequest request)
        {
            var result = await _mediator.Send(new AddCartItemCommand(request));
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Remove an item from a cart.
        /// </summary>
        /// <param name="request">RemoveCartItemRequest</param>
        /// <returns>Success message</returns>
        [HttpDelete("item")]
        [ProducesResponseType(typeof(Result<string>), 200)]
        public async Task<IActionResult> RemoveCartItem([FromBody] RemoveCartItemRequest request)
        {
            var result = await _mediator.Send(new RemoveCartItemCommand(request));
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Decrease the quantity of an item in a cart.
        /// </summary>
        /// <param name="request">DecreaseCartItemQuantityRequest</param>
        /// <returns>Updated Cart DTO</returns>
        [HttpPatch("item/decrease")]
        [ProducesResponseType(typeof(Result<CartDto>), 200)]
        public async Task<IActionResult> DecreaseCartItemQuantity([FromBody] DecreaseCartItemQuantityRequest request)
        {
            var result = await _mediator.Send(new DecreaseCartItemQuantityCommand(request));
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Clear all items from a cart.
        /// </summary>
        /// <param name="cartId">Cart ID</param>
        /// <returns>Updated Cart DTO</returns>
        [HttpDelete("{cartId:guid}/clear")]
        [ProducesResponseType(typeof(Result<CartDto>), 200)]
        public async Task<IActionResult> ClearCart(Guid cartId)
        {
            var result = await _mediator.Send(new ClearCartCommand(cartId));
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Link a cart to a user by session ID.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param >Session ID</param>
        /// <returns>Success message</returns>
        [HttpPost("link")]
        [ProducesResponseType(typeof(Result<string>), 200)]
        public async Task<IActionResult> LinkCartToUser([FromQuery] Guid userId)
        {
            var sessionId = CartSessionExtension.GetOrCreateCartSessionId(HttpContext);
            var result = await _mediator.Send(new LinkCartToUserCommand(userId, sessionId));
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }

}
