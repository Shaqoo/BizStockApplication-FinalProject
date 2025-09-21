using Application.Dto;
using Application.Pagination;
using Application.Queries.Products.GetById;
using Application.Queries.Products.GetByIds;
using Application.Queries.Products.GetProducts;
using Application.Queries.Products.GetProductsByBrand;
using Application.Queries.Products.GetProductsByCategory;
using Application.Queries.Products.GetProductsByPriceRange;
using Application.Queries.Products.GetProductsByStatus;
using Application.Queries.Products.GetProductsByWarehouseId;
using Application.Queries.Products.GetProductsOrderdByPrice;
using Application.Queries.Products.GetProductsOrderedByPriceandCateory;
using Application.Queries.Products.GetProductStats;
using Application.Queries.Products.GetProductWithLowStock;
using Application.Queries.Products.GetRecentlyAddedProducts;
using Application.Queries.Products.GetRelatedProducts;
using Application.Queries.Products.GetRelatedProductsByBrand;
using Application.Queries.Products.GetSuggestions;
using Application.Queries.Products.GetTopRatedProducts;
using Application.Queries.Products.SearchProducts;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public partial class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }



        /// <summary>
        /// Retrieves a paginated list of products for a specific warehouse.
        /// </summary>
        /// <param name="warehouseId">The unique <c>Guid</c> of the warehouse.</param>
        /// <param name="pageRequest">The <c>PageRequest</c> containing pagination parameters.</param>
        /// <remarks>
        /// This endpoint returns products that are stored in the specified warehouse.
        /// </remarks>
        /// <returns>A paginated list of <c>ProductDto</c> objects.</returns>
        [HttpGet("warehouse/{warehouseId:guid}")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByWarehouseId(Guid warehouseId, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetProductsByWarehouseIdQuery(warehouseId, pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves products ordered by price.
        /// </summary>
        /// <param name="ascending">Specifies if results should be in ascending <c>bool</c> order.</param>
        /// <param name="pageRequest">The <c>PageRequest</c> containing pagination parameters.</param>
        /// <remarks>
        /// Set <c>ascending</c> to true for lowest-to-highest pricing, false for highest-to-lowest.
        /// </remarks>
        /// <returns>A paginated list of <c>ProductDto</c> objects ordered by price.</returns>
        [HttpGet("ordered-by-price")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsOrderedByPrice([FromQuery] bool ascending, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetProductsOrderedByPriceQuery(ascending, pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves products by status.
        /// </summary>
        /// <param name="pageRequest">The <c>PageRequest</c> containing pagination parameters.</param>
        /// <param name="productStatus">The <c>ProductStatus</c> enum value to filter products.</param>
        /// <remarks>
        /// This is useful for filtering available, out-of-stock, or discontinued products.
        /// </remarks>
        /// <returns>A paginated list of <c>ProductDto</c> objects matching the given status.</returns>
        [HttpGet("status/{productStatus}")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByStatus([FromQuery] PageRequest pageRequest, ProductStatus productStatus)
        {
            var result = await _mediator.Send(new GetProductsByStatusQuery(pageRequest, productStatus));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves products within a specified price range.
        /// </summary>
        /// <param name="minPrice">The minimum <c>decimal</c> price.</param>
        /// <param name="maxPrice">The maximum <c>decimal</c> price.</param>
        /// <param name="pageRequest">The <c>PageRequest</c> containing pagination parameters.</param>
        /// <remarks>
        /// Both <c>minPrice</c> and <c>maxPrice</c> must be positive and <c>minPrice</c> ≤ <c>maxPrice</c>.
        /// </remarks>
        /// <returns>A paginated list of <c>ProductDto</c> objects in the specified price range.</returns>
        [HttpGet("price-range")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetProductsByPriceRangeQuery(minPrice, maxPrice, pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a product by its unique ID.
        /// </summary>
        /// <param name="id">The unique <c>Guid</c> identifier of the product.</param>
        /// <remarks>
        /// This endpoint returns full product details for the given ID.
        /// </remarks>
        /// <returns>A single <c>ProductDto</c> object.</returns>
        [HttpGet("by-id/{id}")]
        [ProducesResponseType(typeof(Result<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<ProductDto>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of products based on the provided list of product IDs.
        /// </summary>
        /// <param name="ids">A list of product IDs to fetch.</param>
        /// <returns>
        /// Returns a 200 OK response with a list of ProductDto objects if successful.
        /// Returns appropriate error codes if the request is invalid or fails.
        /// </returns>
        [HttpPost("get-by-ids")]
        [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductByIds([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("No product IDs provided.");

            var result = await _mediator.Send(new GetProductByIdsQuery(ids));

            return Ok(result);
        }


        /// <summary>
        /// Retrieves products by category ID.
        /// </summary>
        /// <param name="categoryId">The unique <c>Guid</c> of the category.</param>
        /// <param name="pageRequest">The <c>PageRequest</c> containing pagination parameters.</param>
        /// <remarks>
        /// This endpoint returns all products belonging to the specified category.
        /// </remarks>
        /// <returns>A paginated list of <c>ProductDto</c> objects.</returns>
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByCategoryId(Guid categoryId, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetProductsByCategoryIdQuery(categoryId, pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves products by brand ID.
        /// </summary>
        /// <param name="brandId">The unique <c>Guid</c> of the brand.</param>
        /// <param name="pageRequest">The <c>PageRequest</c> containing pagination parameters.</param>
        /// <remarks>
        /// This endpoint returns all products belonging to the specified brand.
        /// </remarks>
        /// <returns>A paginated list of <c>ProductDto</c> objects.</returns>
        [HttpGet("brand/{brandId}")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByBrandId(Guid brandId, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetProductsByBrandQuery(brandId, pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all products with pagination.
        /// </summary>
        /// <param name="pageRequest">The <c>PageRequest</c> containing pagination parameters.</param>
        /// <remarks>
        /// Use this endpoint to retrieve all products without any filtering.
        /// </remarks>
        /// <returns>A paginated list of <c>ProductDto</c> objects.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetProductsQuery(pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Searches products by a <c>keyword</c>.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The page size for pagination.</param>
        /// <param name="keyword">The search keyword.</param>
        /// <remarks>
        /// This endpoint searches for products that match the given <c>keyword</c> and returns paginated results.
        /// </remarks>
        /// <returns>A paginated list of matching products.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SearchProducts(int pageNumber, int pageSize, string keyword)
        {
            var query = new SearchProductsQuery(new PageRequest { Page = pageNumber, PageSize = pageSize }, keyword);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets top-rated products.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <remarks>
        /// This endpoint returns a paginated list of products with the highest ratings.
        /// </remarks>
        /// <returns>A paginated list of top-rated products.</returns>
        [HttpGet("top-rated")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTopRatedProducts(int pageNumber, int pageSize)
        {
            var query = new GetTopRatedProductsQuery(new PageRequest { Page = pageNumber, PageSize = pageSize });
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets recently added products.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <remarks>
        /// This endpoint retrieves products that were most recently added to the system.
        /// </remarks>
        /// <returns>A paginated list of recently added products.</returns>
        [HttpGet("recent")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRecentlyAddedProducts(int pageNumber, int pageSize)
        {
            var query = new GetRecentlyAddedProductsQuery(new PageRequest { Page = pageNumber, PageSize = pageSize });
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets products with low stock levels.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <remarks>
        /// This endpoint retrieves products that have stock quantities below the defined threshold.
        /// </remarks>
        /// <returns>A paginated list of low-stock products.</returns>
        [HttpGet("low-stock")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductWithLowStock([FromQuery]int pageNumber,[FromQuery] int pageSize)
        {
            var query = new GetProductWithLowStockQuery(new PageRequest { Page = pageNumber, PageSize = pageSize });
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets products by a specific category, ordered by price.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <param name="ascending">Whether to order by ascending (<c>true</c>) or descending (<c>false</c>) price.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <remarks>
        /// This endpoint retrieves products from a given <c>categoryId</c> ordered by price.
        /// </remarks>
        /// <returns>A paginated list of products in the given category.</returns>
        [HttpGet("category/{categoryId}/ordered-by-price")]
        [ProducesResponseType(typeof(Result<PaginatedList<ProductDto>>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductsByCategoryIdOrderedByPrice(Guid categoryId, bool ascending, int pageNumber, int pageSize)
        {
            var query = new GetProductsByCategoryIdOrderedByPriceQuery(categoryId, ascending, new PageRequest {Page  = pageNumber,PageSize = pageSize });
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets product's suggestions by a specific keyword.
        /// </summary>
        /// <param name="keyword">The word to search.</param>
        /// <remarks>
        /// This endpoint retrieves products name using a given <c>keyword</c>.
        /// </remarks>
        /// <returns>A list of products name from the given keyword.</returns>
        [ProducesResponseType(typeof(Result<IEnumerable<string>>), 200)]
        [ProducesResponseType(404)]
        [HttpGet("search-suggestions")]
        public async Task<IActionResult> GetSearchSuggestions([FromQuery]string keyword)
        {
            var query = new GetProductSearchSuggestionsQuery(keyword);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets related products in the same category (excluding the current one).
        /// </summary>
        /// <param name="id">The current product id.</param>
        /// <returns>A list of related products.</returns>
        [ProducesResponseType(typeof(Result<IEnumerable<ProductDto>>), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}/related")]
        public async Task<IActionResult> GetRelatedProducts(Guid id)
        {
            var query = new GetRelatedProductQuery(id);
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Gets related products in the same brand (excluding the current one).
        /// </summary>
        /// <param name="id">The current product id.</param>
        /// <returns>A list of related products.</returns>
        [ProducesResponseType(typeof(Result<IEnumerable<ProductDto>>), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}/related-brands")]
        public async Task<IActionResult> GetRelatedProductsByBrand(Guid id)
        {
            var query = new GetRelatedProductsByBrandQuery(id);
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Retrieves statistics about products including Active, Inactive, Low Stock, and Out of Stock counts.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>Product statistics summary.</returns>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(ProductStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductStats(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductStatsQuery(), cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

    }

}

