using Application.Commands.Products.AddReviewComment;
using Application.Dto;
using Application.Pagination;
using Application.Queries.Reviews.GetProductReviewSummaryQuery;
using Application.Queries.Reviews.GetRatingByProduct;
using Application.Queries.Reviews.GetReviewsByProduct;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

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

        /// <summary>
        /// Retrieves The Average Rating Of A Specific Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("product-ratings/{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRatingByProductId(Guid productId)
        {
            var query = new GetRatingByProductIdQuery(productId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets the review summary (average rating, total ratings, breakdown by stars) for a product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// A <see cref="RatingSummaryDto"/> wrapped in a <see cref="Result{T}"/> 
        /// containing the product’s average rating, total ratings, and star breakdown.
        /// </returns>
        /// <response code="200">Returns the rating summary of the product.</response>
        /// <response code="400">If the product does not exist or an error occurred.</response>
        [HttpGet("summary/{productId:guid}")]
        public async Task<IActionResult> GetProductReviewSummary(Guid productId)
        {
            var result = await _mediator.Send(new GetProductReviewSummaryQuery(productId));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
