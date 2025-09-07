using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.Wishlists.AddItemsToList;
using Application.Commands.Wishlists.CreateWishlist;
using Application.Commands.Wishlists.RemoveItemFromList;
using Application.Dto;
using Application.Pagination;
using Application.Queries.Wishlists.GetItemsByUser;
using Application.Queries.Wishlists.GetWishlistById;
using Application.Queries.Wishlists.GetWishlistByUser;
using MediatR;
using Application.Queries.Wishlists.CheckIfIsInWishlist;

namespace Host.Controllers.V1
{
    namespace Host.Controllers.V1
    {
        [Route("api/v{version:apiVersion}/[controller]")]
        [ApiVersion("1.0")]
        [ApiController]
        [Authorize]
        public class WishlistsController : ControllerBase
        {
            private readonly IMediator _mediator;

            public WishlistsController(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Creates a new wishlist for a user.
            /// </summary>
            /// <param name="command">The create wishlist command containing UserId.</param>
            /// <returns>Result with success or failure message.</returns>
            [HttpPost("create")]
            [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> CreateWishlist([FromBody] CreateWishlistCommand command)
            {
                var result = await _mediator.Send(command);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }

            /// <summary>
            /// Adds a product to the current user's wishlist.
            /// </summary>
            /// <param name="command">The add item to wishlist command containing ProductId.</param>
            /// <returns>Result with success or failure message.</returns>
            [HttpPost("add-item")]
            [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> AddItem([FromBody] AddItemToListCommand command)
            {
                var result = await _mediator.Send(command);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }

            /// <summary>
            /// Removes a product from the current user's wishlist.
            /// </summary>
            /// <param name="command">The remove item from wishlist command containing ProductId.</param>
            /// <returns>Result with success or failure message.</returns>
            [HttpDelete("remove-item")]
            [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> RemoveItem([FromBody] RemoveItemFromListCommand command)
            {
                var result = await _mediator.Send(command);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }

            /// <summary>
            /// Gets a wishlist by its Id.
            /// </summary>
            /// <param name="id">The wishlist Id.</param>
            /// <returns>The wishlist details.</returns>
            [HttpGet("{id:guid}")]
            [ProducesResponseType(typeof(Result<WishlistDto>), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetById(Guid id)
            {
                var result = await _mediator.Send(new GetWishlistByIdQuery(id));
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }

            /// <summary>
            /// Gets the current user's wishlist.
            /// </summary>
            /// <returns>The wishlist details.</returns>
            [HttpGet("me")]
            [ProducesResponseType(typeof(Result<WishlistDto>), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetByUser()
            {
                var result = await _mediator.Send(new GetWishlistByUserIdQuery());
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }

            /// <summary>
            /// Gets all items in the current user's wishlist.
            /// </summary>
            /// <param name="pageRequest">Pagination parameters (PageNumber, PageSize).</param>
            /// <returns>Paginated list of wishlist items.</returns>
            [HttpGet("items")]
            [ProducesResponseType(typeof(Result<PaginatedList<WishlistItemDto>>), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetItems([FromQuery] PageRequest pageRequest)
            {
                var result = await _mediator.Send(new GetWishlistItemsByUserQuery(pageRequest));
                return result.IsSuccess ? Ok(result) : NotFound(result);
            }

            [HttpGet("check/{productId:guid}")]
            [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
            public async Task<IActionResult> CheckIfIsInWishlist(Guid productId)
            {
                var result = await _mediator.Send(new CheckIfIsInWishlistQuery(productId));
                return Ok(result);
            }

        }
    }

}
