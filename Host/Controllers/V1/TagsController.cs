using Application.Commands.Tags.Create;
using Application.Commands.Tags.Delete;
using Application.Commands.Tags.Update;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.Tags.GetAllTags;
using Application.Queries.Tags.GetById;
using Application.Queries.Tags.GetByProductId;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Host.Controllers.V1
{
    /// <summary>
    /// Controller for managing Tags.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="request">The tag creation request.</param>
        /// <returns>The ID of the newly created tag.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var command = new CreateTagCommand(request, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(
                nameof(GetById),                
                new { id = result.Data },     
                result                           
            );
        }

        /// <summary>
        /// Updates an existing tag.
        /// </summary>
        /// <param name="updateTagRequest">The updated tag data.</param>
        /// <returns>A confirmation message.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromBody]UpdateTagRequest updateTagRequest)
        {

            var command = new UpdateTagCommand(updateTagRequest,Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a tag.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>A confirmation message.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteTagCommand(id,Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of all tags.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A paginated list of tags.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedList<TagDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllTagsPaginatedQuery(new PageRequest {Page =  page,PageSize =  pageSize });
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a tag by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <returns>The tag details.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<TagDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<TagDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetTagByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [SwaggerOperation(Summary = "Get all tags for a product",
                          Description = "Retrieves all tags for a specific product with pagination.")]
        [HttpGet("{productId:guid}/productId")]
        [ProducesResponseType(typeof(Result<PaginatedList<TagDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<PaginatedList<TagDto>>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTagsByProductIdPaged([FromRoute] Guid productId,[FromQuery] PageRequest pageRequest)
        {
            var query = new GetTagsByProductIdQuery(productId, pageRequest);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }




    }
}

