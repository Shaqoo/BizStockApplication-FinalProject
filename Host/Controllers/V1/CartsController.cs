using Application.Commands.Carts.AddItem;
using Application.Commands.Carts.Create;
using Application.Commands.Carts.LinkToUser;
using Application.Commands.Carts.RemoveItem;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Extensions;
using Application.Pagination;
using Application.Queries.Carts.GetByCurrentUser;
using Application.Queries.Carts.GetById;
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
        /// Get the current user's cart with paginated items.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters</param>
        /// <returns>Paginated cart DTO</returns>
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



    }

}
