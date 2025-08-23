using Application.Commands.Brands.Create;
using Application.Commands.Brands.Delete;
using Application.Commands.Brands.Update;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.Brands.GetAllBrands;
using Application.Queries.Brands.NewFolder;
using Application.Queries.Brands.Search;
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
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new brand.
        /// </summary>
        /// <param name="dto">The <c>CreateBrandDto</c> containing the brand creation details.</param>
        /// <returns>The ID of the newly created brand.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDto dto)
        {
            var result = await _mediator.Send(new CreateBrandCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return StatusCode((int)HttpStatusCode.NotFound, result.Message);
            return StatusCode((int)HttpStatusCode.Created, result);
        }

        /// <summary>
        /// Updates an existing brand.
        /// </summary>
        /// <param name="dto">The <c>UpdateBrandDto</c> containing updated brand details.</param>
        /// <returns>The ID of the updated brand.</returns>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateBrand([FromBody] UpdateBrandDto dto)
        {
            var result = await _mediator.Send(new UpdateBrandCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return StatusCode((int)HttpStatusCode.NotFound, result.Message);
            return StatusCode((int)HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Deletes an existing brand by <c>id</c>.
        /// </summary>
        /// <param name="id">The <c>Guid</c> ID of the brand to delete.</param>
        /// <param name="requestMetadata">The <c>RequestMetadata</c> containing request details.</param>
        /// <returns>Confirmation message.</returns>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteBrand(Guid id, [FromQuery] RequestMetadata requestMetadata)
        {
            var result = await _mediator.Send(new DeleteBrandCommand(id, requestMetadata));
            if (!result.IsSuccess)
                return StatusCode((int)HttpStatusCode.NotFound, result.Message);
            return StatusCode((int)HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets brand details by <c>id</c>.
        /// </summary>
        /// <param name="id">The <c>Guid</c> ID of the brand to retrieve.</param>
        /// <returns>The <c>BrandDto</c> details.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BrandDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetBrandById(Guid id)
        {
            var result = await _mediator.Send(new GetBrandByIdQuery(id));
            return StatusCode((int)HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets a paginated list of brands.
        /// </summary>
        /// <param name="pageNumber">The <c>int</c> page number (starting from 1).</param>
        /// <param name="pageSize">The <c>int</c> number of items per page.</param>
        /// <returns>A paginated list of <c>BrandDto</c> objects.</returns>
        [HttpGet("paginated")]
        [ProducesResponseType(typeof(PaginatedList<BrandDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPaginatedBrands([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new GetPaginatedBrandsQuery(new PageRequest { Page = pageNumber, PageSize = pageSize }));
            return StatusCode((int)HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Searches brands by <c>keyword</c>.
        /// </summary>
        /// <param name="keyword">The <c>string</c> search keyword.</param>
        /// <param name="pageNumber">The <c>int</c> page number.</param>
        /// <param name="pageSize">The <c>int</c> page size.</param>
        /// <returns>A paginated list of matching <c>BrandDto</c> results.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PaginatedList<BrandDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchBrands([FromQuery] string keyword, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new SearchBrandsQuery(keyword, new PageRequest { Page = pageNumber, PageSize = pageSize }));
            return StatusCode((int)HttpStatusCode.OK, result);
        }
    }


}
