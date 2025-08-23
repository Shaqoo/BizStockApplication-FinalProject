using Application.Commands.Products.AddReviewComment;
using Application.Dto;
using Application.Pagination;
using Application.Queries.Reviews.GetReviewsByProduct;
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
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// API version for this controller.
        /// </summary>
        public const string ApiVersion = "1.0";

        /// <summary>
        /// Retrieves all reviews for a specific product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <param name="pageRequest">The Paging Items Such As The Page Size And Number</param>
        /// <returns>A list of reviews for the specified product.</returns>
        [HttpGet("product/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedList<ProductReviewDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewsByProductId(Guid productId,[FromQuery]PageRequest pageRequest)
        {
            var command = new GetReviewsForProductIdQuery(productId,pageRequest);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return NotFound($"No reviews found for product with ID {productId}");
            return Ok(result);
        }
    }
}
