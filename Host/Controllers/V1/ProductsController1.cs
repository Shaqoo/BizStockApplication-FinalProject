using Application.Commands.Products.ActivateProduct;
using Application.Commands.Products.AddProductReview;
using Application.Commands.Products.AddQuantity;
using Application.Commands.Products.AddReviewComment;
using Application.Commands.Products.AddTags;
using Application.Commands.Products.ArchiveProduct;
using Application.Commands.Products.ChangePrice;
using Application.Commands.Products.Create;
using Application.Commands.Products.ReviewCreatedProduct;
using Application.Commands.Products.UpdateDetails;
using Application.Commands.Products.UpdatePicture;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Host.Controllers.V1
{
    
    public partial class ProductsController : ControllerBase
    {

        /// <summary>
        /// Activates a product by its ID.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to activate (<c>Guid</c>).</param>
        /// <param>Metadata about the request such as IP address and user agent (<c>RequestMetadata</c>).</param>
        /// <remarks>
        /// This endpoint sets the product status to active, making it available for customers or other operations.
        /// </remarks>
        /// <returns>A confirmation message indicating the activation status.</returns>
        [HttpPatch("{productId:guid}/activate")]
        [Authorize]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ActivateProduct(Guid productId)
        {
            var result = await _mediator.Send(new ActivateProductCommand(productId, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return NotFound(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Adds a review for a specific product.
        /// </summary>
        /// <param name="dto">The details of the product review to create (<c>CreateProductReviewDto</c>).</param>
        /// <param >Metadata about the request such as IP address and user agent (<c>RequestMetadata</c>).</param>
        /// <remarks>
        /// This endpoint allows users to submit a review for a product, including rating and comments.
        /// </remarks>
        /// <returns>The unique identifier (<c>Guid</c>) of the newly created product review.</returns>
        [HttpPost("reviews")]
        [Authorize]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddProductReview([FromBody] CreateProductReviewDto dto)
        {
            var result = await _mediator.Send(new AddProductReviewCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }


        /// <summary>
        /// Adds quantity to an existing product's stock.
        /// </summary>
        /// <param name="dto">The details of the quantity to add (<c>AddProductQuantityDto</c>).</param>
        /// <param>Metadata about the request such as IP address and user agent (<c>RequestMetadata</c>).</param>
        /// <remarks>
        /// This endpoint allows authorized users to increase the stock quantity of a product in the inventory.
        /// </remarks>
        /// <returns>A confirmation message (<c>string</c>) indicating the operation's success.</returns>
        [HttpPut("add-quantity")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddProductQuantity([FromBody] AddProductQuantityDto dto)
        {
            var result = await _mediator.Send(new AddProductQuantityCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }


        /// <summary>
        /// Adds a comment to an existing product review.
        /// </summary>
        /// <param name="reviewId">The ID of the review to comment on (<c>Guid</c>).</param>
        /// <param name="comment">The comment text to add (<c>string</c>).</param>
        /// <param>Metadata about the request such as IP address and user agent (<c>RequestMetadata</c>).</param>
        /// <remarks>
        /// This endpoint allows authorized users to add comments to a specific product review. 
        /// Comments can provide additional feedback or clarification on the review.
        /// </remarks>
        /// <returns>A confirmation message (<c>string</c>) indicating the operation's success.</returns>
        [HttpPut("add-review-comment/{reviewId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddReviewComment([FromRoute] Guid reviewId, [FromBody] string comment)
        {
            var result = await _mediator.Send(new AddReviewCommentCommand(reviewId, comment,Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }


        /// <summary>
        /// Adds tags to a product.
        /// </summary>
        /// <param name="addProductTag">The tags to add along with the product ID (<c>AddProductTagDto</c>).</param>
        /// <param >Metadata about the request such as IP address and user agent (<c>RequestMetadata</c>).</param>
        /// <remarks>
        /// This endpoint allows authorized users to associate multiple tags with a product. 
        /// Adding tags helps in categorizing products and improving search functionality.
        /// </remarks>
        /// <returns>A confirmation message (<c>string</c>) indicating the operation's success.</returns>
        [HttpPost("add-product-tags")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddProductTags([FromBody] AddProductTagDto addProductTag)
        {
            var result = await _mediator.Send(new AddProductTagsCommand(addProductTag, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }


        /// <summary>
        /// Archives a product.
        /// </summary>
        /// <param name="productId">The ID of the product to archive (<c>Guid</c>).</param>
        /// <param >Metadata about the request such as IP address and user agent (<c>RequestMetadata</c>).</param>
        /// <remarks>
        /// Archiving a product marks it as inactive in the system without deleting it. 
        /// This allows the product to be restored later if needed and prevents it from appearing in active listings.
        /// </remarks>
        /// <returns>A confirmation message (<c>string</c>) indicating the operation's success.</returns>
        [HttpDelete("products/{productId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ArchiveProduct(Guid productId)
        {
            var result = await _mediator.Send(new ArchiveProductCommand(productId, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Changes the price of a product.
        /// </summary>
        /// <param name="change">The new price details (<c>ChangeProductPriceDto</c>).</param>
        /// <param >Metadata about the request such as IP address and user agent (<c>RequestMetadata</c>).</param>
        /// <remarks>
        /// Updates the price of the specified product in the system. 
        /// Ensure that the new price meets the business rules and validations before calling this endpoint.
        /// </remarks>
        /// <returns>A confirmation message (<c>string</c>) indicating whether the price change was successful.</returns>
        [HttpPut("change-product-price")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangeProductPrice([FromBody] ChangeProductPriceDto change)
        {
            var result = await _mediator.Send(new ChangeProductPriceCommand(change, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new product with optional picture upload.
        /// </summary>
        /// <param name="requestModel"><c>requestModel</c> The product details including picture file.</param>
        /// <param ><c>requestMetadata</c> Metadata about the request (IP, user agent, etc.).</param>
        /// <remarks>Accepts multipart/form-data to allow file upload along with product details.</remarks>
        /// <returns>The created product details.</returns>
        [HttpPost("create-product")]
        [Authorize]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequestModel requestModel)
        {
            var result = await _mediator.Send(new CreateProductCommand(requestModel, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Data!.Id }, result);
        }

        /// <summary>
        /// Allows an admin to review a newly created product before it becomes active.
        /// </summary>
        /// <param ><c>productId</c> - The ID of the product to review.</param>
        /// <param name="reviewDto"><c>reviewDto</c> - The review details provided by the admin.</param>
        /// <remarks>
        /// This endpoint should be used to approve or reject products submitted by inventory managers. Only users with the "Admin" role can perform this action.
        /// </remarks>
        /// <returns>Returns a success message if the product review is processed successfully.</returns>
        [HttpPatch("review-product")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReviewProduct([FromBody] ReviewCreatedProductDto reviewDto)
        {
            var result = await _mediator.Send(new ReviewCreatedProductCommand(reviewDto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return StatusCode((int)HttpStatusCode.BadRequest, result.Message);
            return Ok(result);
        }


        /// <summary>
        /// Updates the details of an existing product.
        /// </summary>
        /// <param name="productId"><c>productId</c> - The ID of the product to update.</param>
        /// <param name="productDetails"><c>productDetails</c> - The new product details to apply.</param>
        /// <param ><c>requestMetadata</c> - Metadata about the request (IP, user agent, etc.).</param>
        /// <remarks>
        /// Only authorized users can update product details. This endpoint modifies existing product information like name, description, price, or stock.
        /// </remarks>
        /// <returns>Returns a success message if the product details were updated successfully.</returns>
        [HttpPut("update-product-details/{productId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProductDetails(
            Guid productId,
            [FromBody] UpdateProductDetailsDto productDetails)
        {
            var result = await _mediator.Send(new UpdateProductDetailsCommand(productId, productDetails, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return StatusCode((int)HttpStatusCode.BadRequest, result.Message);
            return Ok(result);
        }

        /// <summary>
        /// Updates the picture of an existing product.
        /// </summary>
        /// <param name="pictureDto"><c>pictureDto</c> - The new picture data for the product.</param>
        /// <param ><c>requestMetadata</c> - Metadata about the request (IP, user agent, etc.).</param>
        /// <remarks>
        /// Only authorized users can update product pictures. This endpoint allows updating the main product image or gallery images.
        /// The request should be sent as <c>multipart/form-data</c> if including files.
        /// </remarks>
        /// <returns>Returns a success message if the product picture was updated successfully.</returns>
        [HttpPut("update-product-picture")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProductPicture(
            [FromForm] UpdateProductPictureDto pictureDto)
        {
            var result = await _mediator.Send(new UpdateProductPictureCommand(pictureDto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return StatusCode((int)HttpStatusCode.BadRequest, result.Message);
            return Ok(result);
        }


    }
}
