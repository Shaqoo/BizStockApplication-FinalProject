using Application.Commands.Specifications.AddProductSpecification;
using Application.Commands.Specifications.CreateSpecification;
using Application.Commands.Specifications.DeleteSpecification;
using Application.Commands.Specifications.RemoveProductSpecification;
using Application.Commands.Specifications.UpdateProductSpecification;
using Application.Commands.Specifications.UpdateSpecification;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Queries.Specifications.GetAllSpecifications;
using Application.Queries.Specifications.GetProductSpecificationsByProductId;
using Application.Queries.Specifications.GetSpecificationById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SpecificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpecificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new Specification
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateSpecification([FromBody] CreateSpecificationRequest request)
        {
            var result = await _mediator.Send(new CreateSpecificationCommand(request));
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update an existing Specification
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateSpecification(Guid id, [FromBody] UpdateSpecificationRequest request)
        {
            request.SpecificationId = id;
            var result = await _mediator.Send(new UpdateSpecificationCommand(request));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Delete a Specification
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteSpecification(Guid id)
        {
            var result = await _mediator.Send(new DeleteSpecificationCommand(id));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Add Specification to Product
        /// </summary>
        [HttpPost("productspecifications")]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddProductSpecification([FromBody] AddProductSpecificationRequest request)
        {
            var result = await _mediator.Send(new AddProductSpecificationCommand(request));
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Update Product Specification
        /// </summary>
        [HttpPut("productspecifications/{id:guid}")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProductSpecification(Guid id, [FromBody] UpdateProductSpecificationRequest request)
        {
            request.ProductSpecificationId = id;
            var result = await _mediator.Send(new UpdateProductSpecificationCommand(request));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Remove a Product Specification
        /// </summary>
        [HttpDelete("productspecifications/{id:guid}")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> RemoveProductSpecification(Guid id)
        {
            var result = await _mediator.Send(new RemoveProductSpecificationCommand(id));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Get a specification by its unique Id.
        /// </summary>
        /// <param name="id">The specification Id.</param>
        /// <returns>A specification DTO.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<SpecificationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSpecificationById(Guid id)
        {
            var result = await _mediator.Send(new GetSpecificationByIdQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Get all specifications.
        /// </summary>
        /// <returns>List of specification DTOs.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<List<SpecificationDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSpecifications()
        {
            var result = await _mediator.Send(new GetAllSpecificationsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get all product specifications by Product Id.
        /// </summary>
        /// <param name="productId">The product Id.</param>
        /// <returns>List of product specifications for the product.</returns>
        [HttpGet("product/{productId:guid}")]
        [ProducesResponseType(typeof(Result<ProductSpecificationListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductSpecificationsByProductId(Guid productId)
        {
            var result = await _mediator.Send(new GetProductSpecificationsByProductIdQuery(productId));
            return Ok(result);
        }
    }

}
