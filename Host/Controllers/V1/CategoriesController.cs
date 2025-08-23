using Application.Commands.Categories.Create;
using Application.Commands.Categories.Delete;
using Application.Commands.Categories.Move;
using Application.Commands.Categories.Update;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.Categories.GetById;
using Application.Queries.Categories.GetCategoryHierarchy;
using Application.Queries.Categories.GetFilteredCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves the full category tree structure.
        /// </summary>
        /// <remarks>
        /// This endpoint returns all categories in a hierarchical tree format.
        /// </remarks>
        /// <returns>A list of <c>CategoryTreeDto</c> objects representing the category hierarchy.</returns>
        /// <response code="200">Returns the category tree list.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [HttpGet("tree")]
        [ProducesResponseType(typeof(Result<List<CategoryTreeDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<List<CategoryDto>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCategoryTree()
        {
            var result = await _mediator.Send(new GetCategoryTreeQuery());
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category (<c>Guid</c>).</param>
        /// <remarks>
        /// Use this endpoint to fetch detailed information about a specific category.
        /// </remarks>
        /// <returns>A <c>CategoryDto</c> object containing the category details.</returns>
        /// <response code="200">Returns the category data.</response>
        /// <response code="404">If the category with the given <c>id</c> is not found.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<CategoryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<CategoryDto>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result<CategoryDto>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of categories based on filtering options.
        /// </summary>
        /// <param name="filter">The filtering parameters (<c>GetCategoriesFilter</c>).</param>
        /// <remarks>
        /// This endpoint allows filtering categories by depth, search term, and pagination options.
        /// </remarks>
        /// <returns>A paginated list of <c>CategoryDto</c> objects matching the filter criteria.</returns>
        /// <response code="200">Returns the filtered list of categories.</response>
        /// <response code="400">If the filter parameters are invalid.</response>
        /// <response code="500">If an unexpected server error occurs.</response>
        [HttpGet("filter")]
        [ProducesResponseType(typeof(Result<PaginatedList<CategoryDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<PaginatedList<CategoryDto>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result<PaginatedList<CategoryDto>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetFilteredCategories([FromQuery] GetCategoriesFilter filter)
        {
            var result = await _mediator.Send(new GetFilteredCategoriesQuery(filter));
            return Ok(result);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="dto">The category creation details (<c>CreateCategoryDto</c>).</param>
        /// <remarks>
        /// This endpoint allows you to create a new category by providing name, description, and optional parent category.
        /// </remarks>
        /// <returns>The created <c>CategoryDto</c> object.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<CategoryDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Result<CategoryDto>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(dto));
            if (!result.IsSuccess)
                return BadRequest(result);
            return StatusCode((int)HttpStatusCode.Created, result);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category (<c>Guid</c>).</param>
        /// <param name="dto">The updated category details (<c>UpdateCategoryDto</c>).</param>
        /// <remarks>
        /// Use this endpoint to modify the name, description, or other details of an existing category.
        /// </remarks>
        /// <returns>The updated <c>CategoryDto</c> object.</returns>
        [HttpPut("{categoryId:guid}")]
        [ProducesResponseType(typeof(Result<CategoryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<CategoryDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] UpdateCategoryDto dto)
        {
            var result = await _mediator.Send(new UpdateCategoryCommand(dto, categoryId));
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Moves a category to a new parent or position.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category (<c>Guid</c>).</param>
        /// <param name="dto">The move details (<c>MoveCategoryDto</c>).</param>
        /// <remarks>
        /// This endpoint allows changing the parent category or order of a category within the hierarchy.
        /// </remarks>
        /// <returns>The updated <c>CategoryDto</c> object.</returns>
        [HttpPut("{categoryId:guid}/move")]
        [ProducesResponseType(typeof(Result<CategoryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<CategoryDto>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> MoveCategory(Guid categoryId, [FromBody] MoveCategoryDto dto)
        {
            var result = await _mediator.Send(new MoveCategoryCommand(dto, categoryId));
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an existing category by ID.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category to delete (<c>Guid</c>).</param>
        /// <remarks>
        /// Deleting a category will remove it permanently. Ensure there are no dependent entities that rely on this category.
        /// </remarks>
        /// <returns>A confirmation message.</returns>
        [HttpDelete("{categoryId:guid}")]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(categoryId));
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }

}
